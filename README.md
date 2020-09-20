<h1>SquirrelAutoUpdater</h1>

- [Nugets](#nugets)
- [Steps](#steps)
- [References](#references)

# Nugets
* Squirrel.Windows
* NuGet.CommandLine
  
# Steps
1. Add the below code to AssemblyInfo.cs
   ```csharp
   [assembly: AssemblyMetadata("SquirrelAwareVersion", "1")]
2. Add the below code in program main method
    ```csharp
    using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/r1-prototype-studies/SquirrelAutoUpdater/releases/latest"))
            {
        await mgr.Result.UpdateApp();
    }
3. Create a .nuspec file
    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
    <metadata>
        <id>MyApp</id>
        <!-- version will be replaced by MSBuild -->
        <version>0.0.0.0</version>
        <title>title</title>
        <authors>authors</authors>
        <description>description</description>
        <requireLicenseAcceptance>false</requireLicenseAcceptance>
        <copyright>Copyright 2016</copyright>
        <dependencies />
    </metadata>
    <files>
        <file src="*.*" target="lib\net45\" exclude="*.pdb;*.nupkg;*.vshost.*"/>
    </files>
    </package>
4. Add the below build target to the project
    ```xml
    <Target Name="AfterBuild" Condition=" '$(Configuration)' == 'Release'">
        <GetAssemblyIdentity AssemblyFiles="$(TargetPath)">
            <Output TaskParameter="Assemblies" ItemName="myAssemblyInfo"/>
        </GetAssemblyIdentity>
        <Exec Command="nuget pack MyApp.nuspec -Version %(myAssemblyInfo.Version) -Properties Configuration=Release -OutputDirectory $(OutDir) -BasePath $(OutDir)" />
        <Exec Command="squirrel --releasify $(OutDir)MyApp.$([System.Version]::Parse(%(myAssemblyInfo.Version)).ToString(3)).nupkg" />
    </Target>
5.


# References
* https://github.com/A9T9/Free-OCR-API-CSharp
* https://github.com/Squirrel/Squirrel.Windows
* https://intellitect.com/deploying-app-squirrel/

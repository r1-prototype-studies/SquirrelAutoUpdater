<h1>SquirrelAutoUpdater</h1>

- [Nugets](#nugets)
- [Steps](#steps)
- [References](#references)

# Nugets
* Squirrel.Windows
* NuGet.CommandLine
  
# Steps
1. Add the below code to AssemblyInfo.cs -- Remove this for default behavior
   ```csharp
   [assembly: AssemblyMetadata("SquirrelAwareVersion", "1")]
2. Add the below code in program main method
    ```csharp
    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    using (var updateManager = await UpdateManager.GitHubUpdateManager("https://github.com/r1-prototype-studies/SquirrelAutoUpdater"))
    {
      MessageBox.Show(updateManager.RootAppDirectory);
      MessageBox.Show($"Current version: {updateManager.CurrentlyInstalledVersion().Version}");
      UpdateInfo updateInfo = await updateManager.CheckForUpdate();
      MessageBox.Show("UpdateInfo version: " + updateInfo.FutureReleaseEntry.Version.ToString());
      var releaseEntry = await updateManager.UpdateApp();
      //MessageBox.Show($"Update Version: {releaseEntry?.Version.ToString() ?? "No update"}");
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
      <Output TaskParameter="Assemblies" ItemName="myAssemblyInfo" />
    </GetAssemblyIdentity>
    <ItemGroup>
      <NuGetExe Include="..\packages\NuGet.CommandLine.*\tools\nuget.exe" />
      <SquirrelExe Include="..\packages\Squirrel.Windows.*\tools\squirrel.exe" />
    </ItemGroup>
    <PropertyGroup>
      <ReleaseDir>..\Releases\</ReleaseDir>
      <SetupIcon>..\Assets\setup.ico</SetupIcon>
      <SquirrelParams>--no-msi</SquirrelParams>
      <AppVersion>$([System.Version]::Parse(%(myAssemblyInfo.Version)).ToString(3))</AppVersion>
    </PropertyGroup>
    <Error Condition="!Exists(%(NuGetExe.FullPath))" Text="You are trying to use the NuGet.CommandLine package, but it is not installed. Please install NuGet.CommandLine from the Package Manager." />
    <Error Condition="!Exists(%(SquirrelExe.FullPath))" Text="You are trying to use the Squirrel.Windows package, but it is not installed. Please install Squirrel.Windows from the Package Manager." />
    <Exec Command="&quot;%(NuGetExe.FullPath)&quot; pack TestApp.nuspec -Version $(AppVersion) -Properties Configuration=Release -OutputDirectory $(OutDir) -BasePath $(OutDir)" />
    <Exec Command="%(SquirrelExe.FullPath) --releasify $(OutDir)TestApp.$(AppVersion).nupkg --releaseDir=$(ReleaseDir) --setupIcon $(SetupIcon) $(SquirrelParams)" />
  </Target>
5. Create a Release in Github. Upload the release, full and delta nuget packages.
  Refer https://github.com/r1-prototype-studies/SquirrelAutoUpdater/releases


# References
* https://github.com/A9T9/Free-OCR-API-CSharp
* https://github.com/Squirrel/Squirrel.Windows
* https://intellitect.com/deploying-app-squirrel/
* https://madstt.dk/squirrel-the-basic/
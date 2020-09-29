using Squirrel;
using Squirrel.SimpleSplat;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCRAPITest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            UpdateApplication();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private async static void UpdateApplication()
        {
            try
            {

                MessageBox.Show("Trying to update");
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

                /*using (var updateManager = new UpdateManager(@"D:\Work\SourceCode\Prototypes\SquirrelAutoUpdater\src\SampleCode\Releases"))
                {
                    MessageBox.Show($"Current version: {updateManager.CurrentlyInstalledVersion().Version}");
                    UpdateInfo updateInfo = updateManager.CheckForUpdate().Result;
                    MessageBox.Show("UpdateInfo version: " + updateInfo.CurrentlyInstalledVersion.Version.ToString());
                    var releaseEntry = await updateManager.UpdateApp();
                    MessageBox.Show($"Update Version: {releaseEntry?.Version.ToString() ?? "No update"}");
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.ToString());
            }

        }
    }
}

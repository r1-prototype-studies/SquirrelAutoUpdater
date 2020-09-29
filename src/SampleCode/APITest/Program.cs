using Squirrel;
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

        private static void UpdateApplication()
        {
            try
            {

                MessageBox.Show("Trying to update");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                /*using (var updateManager = UpdateManager.GitHubUpdateManager("https://github.com/r1-prototype-studies/SquirrelAutoUpdater"))
                {
                    updateManager.Result.UpdateApp();
                }*/

                using (var updateManager = new UpdateManager(@"D:\Work\SourceCode\Prototypes\SquirrelAutoUpdater\src\SampleCode\Releases"))
                {
                    MessageBox.Show($"Current version: {updateManager.CurrentlyInstalledVersion().Version}");
                    UpdateInfo updateInfo = updateManager.CheckForUpdate().Result;
                    MessageBox.Show(updateInfo.CurrentlyInstalledVersion.Version.ToString());
                    var releaseEntry = updateManager.UpdateApp().Result;
                    MessageBox.Show($"Update Version: {releaseEntry?.Version.ToString() ?? "No update"}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}

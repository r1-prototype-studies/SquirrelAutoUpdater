﻿using Squirrel;
using System;
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
            using (var updateManager = UpdateManager.GitHubUpdateManager("https://github.com/r1-prototype-studies/SquirrelAutoUpdater"))
            {
                updateManager.Result.UpdateApp();
            }
        }
    }
}

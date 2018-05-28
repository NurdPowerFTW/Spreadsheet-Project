// Created by Wei-Tung, Tang, Chen-Chia Wang
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    static class Launch
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Get the context and run on from inside of it
            var context = SpreadsheetApplicationContext.GetContext();
            context.RunNew(null);
            Application.Run(context);
        }
    }
}

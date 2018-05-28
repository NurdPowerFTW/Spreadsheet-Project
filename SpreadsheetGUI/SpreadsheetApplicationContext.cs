// Created by Wei-Tung, Tang, Chen-Chia Wang
using SS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    class SpreadsheetApplicationContext : ApplicationContext
    {
        // Number of open forms
        private int windowCount = 0;

        // Singleton ApplicationContext
        private static SpreadsheetApplicationContext context;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadsheetApplicationContext()
        {
        }

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        public static SpreadsheetApplicationContext GetContext()
        {
            if (context == null)
            {
                context = new SpreadsheetApplicationContext();
            }
            return context;
        }

        /// <summary>
        /// Runs a form in this application context
        /// </summary>
        public void RunNew(string pathName)
        {
            Spreadsheet spreadsheet = null;
            string name = "Spreadsheet" + (windowCount + 1);

            if (pathName == null) 
                spreadsheet = new Spreadsheet(new Regex(@"^[A-Z][1-9]\d?$"));
            else 
            {
                name = Path.GetFileName(pathName);

                using (StreamReader reader = new StreamReader(pathName))
                    spreadsheet = new Spreadsheet(reader, new Regex(@"^[A-Z][1-9]\d?$"));
            }

            SpreadsheetWindow window = new SpreadsheetWindow();
            new Controller(window, spreadsheet, name);

            window.FormClosed += (obj, eve) => { if (--windowCount <= 0) ExitThread(); };

            window.Show();

            windowCount++;
        }
    }
}

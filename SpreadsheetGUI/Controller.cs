using Formulas;
using SS;
using SSGui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public class Controller
    {
        /// <summary>
        /// Declares an interface for communication between view and controller
        /// </summary>
        private ISpreadsheetView view;

        /// <summary>
        /// Creates a sheet reference
        /// </summary>
        private Spreadsheet sheet = new Spreadsheet();

        /// <summary>
        /// Used to initialize interface, sheet reference and window title for Controller's
        /// constructors. Also, tie all the events defined in SpreadsheetWindow
        /// </summary>
        /// <param name="window"></param>
        /// <param name="sheet"></param>
        /// <param name="title"></param>
        private void Inititalize(ISpreadsheetView window, Spreadsheet sheet, string title)
        {
            view = window;

            if (sheet == null)
                sheet = new Spreadsheet();
            else
                this.sheet = sheet;

            view.FileChosenEvent += HandleFileChosen;
            view.NewEvent += HandleNew;
            view.SelectEvent += HandleSelectionChanged;
            view.EnterCellContentEvent += HandleEditBoxText;
            view.FileSaveEvent += HandleSaveFile;
            view.UnsavedCloseEvent += HandleUnSavedClose;
            view.FileCloseEvent += HandleClose;
            view.Title = title;
        }

        /// <summary>
        /// Begins controlling window without supplying a sheet reference for a new window 
        /// </summary>
        public Controller(ISpreadsheetView window, string title)
        {
            Inititalize(window, null, title);

        }

        /// <summary>
        /// Open a file and fill the speadsheet window with the file information
        /// </summary>
        /// <param name="window"></param>
        /// <param name="sheet"></param>
        /// <param name="title"></param>
        public Controller(ISpreadsheetView window, Spreadsheet sheet, string title)
        {
            Inititalize(window, sheet, title);

            int[] temp;

            // Fill all nonempty cell with value in the source file
            foreach (string cell in sheet.GetNamesOfAllNonemptyCells())
            {
                GetCellNumber(cell, out int col, out int row);
                temp = new int[2] { col, row };
                view.SelectedCellAddress = temp;
                view.SetCellValueOnPanel(temp, sheet.GetCellValue(cell).ToString());
            }

            
            HandleSelectionChanged(new int[2] { 0, 0 });

        }


        /// <summary>
        /// update the focus cell info on the actual spreadsheet model
        /// </summary>
        /// <param name="address"></param>
        private void HandleSelectionChanged(int[] address)
        {
            view.SelectedContents = sheet.GetCellContents(GetCellAlphabet(address[0], address[1])).ToString();
            view.SelectedValue = sheet.GetCellValue(GetCellAlphabet(address[0], address[1])).ToString();
            view.SelectedCellAddress = address;
        }

        /// <summary>
        /// helper, translate the cell address from computer read-friendly to human readable address
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetCellAlphabet(int col, int row)
        {
            col += 'A';
            row += 1;
            return ((char)col).ToString() + row;

        }

        /// <summary>
        /// convert it back from user readable name to code name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void GetCellNumber(string name, out int col, out int row)
        {
            col = (int)name.ToString().First() - (int)'A';
            int last = (int)Char.GetNumericValue(name.Last());
            row = (int)last - 1;
        }

        /// <summary>
        /// update the focus cell content with user's request and update the value and display of the spreadsheet with the updated value of all effected cells
        /// </summary>
        /// <param name="content"></param>
        private void HandleEditBoxText(string content)
        {
            int col = view.SelectedCellAddress[0];
            int row = view.SelectedCellAddress[1];
            List<string> list = new List<string>();

            try
            {
                list = sheet.SetContentsOfCell(GetCellAlphabet(col, row), content).ToList();
            }
            catch (Exception e)
            {
                view.Message = e.Message;
            }


            foreach (string cell in list)
            {
                GetCellNumber(cell, out col, out row);
                int[] temp = new int[2] { col, row };
                view.SetCellValueOnPanel(temp, sheet.GetCellValue(cell).ToString());
            }
            HandleSelectionChanged(view.SelectedCellAddress);
        }


        /// <summary>
        /// Handles a request to open a file, throwing exception if anything goes wrong
        /// </summary>
        private void HandleFileChosen(String filename)
        {
            try
            {
                SpreadsheetApplicationContext.GetContext().RunNew(filename);
            }
            catch (Exception e)
            {
                view.ShowFileOpenErrorBox(e);
            }

        }

        /// <summary>
        /// helper method to output and save the file
        /// </summary>
        /// <param name="filePath"></param>
        private void HandleSaveFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
                sheet.Save(writer);
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            view.DoClose();
        }

        /// <summary>
        /// Handles a request to open a new window.
        /// </summary>
        private void HandleNew()
        {
            view.OpenNew();
        }

        /// <summary>
        /// Shows a window if unsaved progress is detected
        /// </summary>
        /// <param name="e"></param>
        private void HandleUnSavedClose(FormClosingEventArgs e)
        {
            view.ShowUnsavedClosePromptBox(e);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public class SpreadsheetWindowStub : ISpreadsheetView
    {

        // Keeps track of selected cell.
        private int[] cellAddress; 
        // This 2D string array stores all cells' content with 26 cols and 99 rows
        private string[,] cellContents;
        // This 2D string array stores all cells' value with 26 cols and 99 rows
        private string[,] cellValues;
        // A bool value to check if spreadsheet has been modified
        private bool modified;

        public SpreadsheetWindowStub()
        {
            cellAddress = new int[] { 0, 0 };
            cellContents = new string[26, 99];
            cellValues = new string[26, 99];

        }

        public int[] SelectedCellAddress
        {
            get
            {
                return new int[] { cellAddress[0], cellAddress[1] };
            }

            set
            {
                cellAddress = new int[] { value[0], value[1] };
            }
        }

        public string SelectedValue { set { } }
        
        public string SelectedContents
        {
            get
            {
                if (cellContents[cellAddress[0], cellAddress[1]] != null)
                    return cellContents[cellAddress[0], cellAddress[1]];

                return "";
            }

            set
            {
                cellContents[cellAddress[0], cellAddress[1]] = value;
            }
        }

        public string SelectedValueSim
        {
            get
            {
                if (cellValues[cellAddress[0], cellAddress[1]] != null)
                    return cellValues[cellAddress[0], cellAddress[1]];
                return "";
            }
        }

        public bool Modified
        {
            set
            {
                modified = value;
            }
        }

        public string Title { set { } }
            

        public string Message { set { } }

        public event Action<string> FileChosenEvent;
        public event Action<string> FileSaveEvent;
        public event Action NewEvent;
        public event Action<int[]> SelectEvent;
        public event Action<string> EnterCellContentEvent;
        public event Action FileCloseEvent;
        public event Action<FormClosingEventArgs> UnsavedCloseEvent;

        public void DoClose()
        {
            
        }

        public void OpenNew()
        {
            
        }

        public void SetCellValueOnPanel(int[] cell, string value)
        {
            cellValues[cell[0], cell[1]] = value;
        }

        /// <summary>
        /// focus on the cell that is clicked
        /// </summary>
        /// <param name="cell"></param>
        public void ChooseCell(int[] cell)
        {
            SelectEvent(cell);
        }

        /// <summary>
        /// enter cell content into the targeted cell
        /// </summary>
        /// <param name="contents"></param>
        public void PutCellContent(string contents)
        {
            EnterCellContentEvent(contents);
        }

        /// <summary>
        /// Save the file to the targeted path
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveFile(string filePath)
        {
            FileSaveEvent(filePath);
        }

        /// <summary>
        /// Open the file to the targeted path
        /// </summary>
        /// <param name="filePath"></param>
        public void OpenFile(string filePath)
        {
            if (FileChosenEvent != null)
                FileChosenEvent(filePath);
        }

        /// <summary>
        /// open a new file
        /// </summary>
        public void OpenNewFile()
        {
            if (NewEvent != null)
                NewEvent();
        }

        /// <summary>
        /// allow user to close file
        /// </summary>
        public void CloseFile()
        {
            if (FileCloseEvent != null)
                FileCloseEvent();
        }

        public void ShowFileOpenErrorBox(Exception e)
        {
            FileChosenEvent(null);
        }

        public void ShowUnsavedClosePromptBox(FormClosingEventArgs e)
        {
            
        }

        
    }
}

// Created by Wei-Tung, Tang, Chen-Chia Wang
using SSGui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// This class is responsible for creating GUI elements for Spreadsheet 
    /// </summary>
    public partial class SpreadsheetWindow : Form, ISpreadsheetView
    {
        /// <summary>
        /// Fired when a file is chosen with a file dialog.
        /// The parameter is the chosen filename.
        /// </summary>
        public event Action<string> FileChosenEvent;

        /// <summary>
        /// event when the user attempt to save the file
        /// </summary>
        public event Action<string> FileSaveEvent;

        /// <summary>
        /// event when the user attempt to close the file
        /// </summary>
        public event Action FileCloseEvent;

        /// <summary>
        /// event when the program detects the file isn't saved and the user is trying to close the program
        /// </summary>
        public event Action<FormClosingEventArgs> UnsavedCloseEvent;

        /// <summary>
        /// fired when a new action is requested.
        /// </summary>
        public event Action NewEvent;

        /// <summary>
        /// the event when the user is trying to select a cell
        /// </summary>
        public event Action<int[]> SelectEvent;

        /// <summary>
        /// the event when user is trying to enter a content into a cell
        /// </summary>
        public event Action<string> EnterCellContentEvent;

        /// <summary>
        /// constructor that intitialize evenets and some intance variables
        /// </summary>
        public SpreadsheetWindow()
        {
            InitializeComponent();
            IsModified = false;
            Saved = false;
            spreadsheetPanel.SelectionChanged += PerformClickOnPanel;
            SelectedCellAddress = new int[2];
        }

        /// <summary>
        /// retrun and update the editbox content
        /// </summary>
        public string SelectedContents
        {
            get
            {
                return EditBox.Text;
            }

            set
            {
                EditBox.Text = value;
            }
        }

        /// <summary>
        /// update the view box value
        /// </summary>
        public string SelectedValue
        {
            set
            {
                ViewBox.Text = value;
            }
        }

        /// <summary>
        /// keep track of the file status (modeified/unmodified)
        /// </summary>
        private bool IsModified { get; set; }

        /// <summary>
        /// This bool value keep track of file saving status
        /// </summary>
        private bool Saved { get; set; }

        /// <summary>
        /// set value on the targeted cell
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        public void SetCellValueOnPanel(int[] cell, string value)
        {
            spreadsheetPanel.SetValue(cell[0], cell[1], value);
        }



        /// <summary>
        /// Sets the title in the UI
        /// </summary>
        public string Title
        {
            set { Text = value; }
        }

        /// <summary>
        /// Shows the message in the UI.
        /// </summary>
        public string Message
        {
            set { MessageBox.Show(value); }
        }

        /// <summary>
        /// set/return the focus cell address
        /// </summary>
        public int[] SelectedCellAddress { get; set; }

        /// <summary>
        /// every click on the panel will check the value of the focus cell and update/display the cell value in the textbox
        /// </summary>
        /// <param name="panel"></param>
        private void PerformClickOnPanel(SpreadsheetPanel panel)
        {
            panel.GetSelection(out int col, out int row);
            panel.GetValue(col, row, out string value);
            UpdateTextBoxInfo(col, row);
        }

        /// <summary>
        /// helper method to update the targeted cell data
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void UpdateTextBoxInfo(int col, int row)
        {
            int[] cell = new int[2] { col, row };
            SelectEvent(cell);
        }
        /// <summary>
        /// Closes this window
        /// </summary>
        public void DoClose()
        {
            Close();
        }

        /// <summary>
        /// Opens a new Analysis window.
        /// </summary>
        public void OpenNew()
        {
            SpreadsheetApplicationContext.GetContext().RunNew(null);
        }


        /// <summary>
        /// allow user to open an exisiting file and update the content in the speadsheet for view/edits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile_Click(object sender, EventArgs e)
        {

            DialogResult result = openFileDialog1.ShowDialog();

            openFileDialog1.Filter = "Spreadsheet Files (*.ss)|*.ss|All Files|*.*";
            openFileDialog1.FilterIndex = 1;

            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileChosenEvent != null)
                {
                    FileChosenEvent(openFileDialog1.FileName);
                }

            }
        }

        /// <summary>
        /// Handles the Click event of the newItem control.
        /// </summary>
        private void NewItem_Click(object sender, EventArgs e)
        {

            if (NewEvent != null)
            {
                NewEvent();
            }

        }

        /// <summary>
        /// when the save button is clicked, pop out a window that allows user to save to choose any dictory to save on the computer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFile_Click(object sender, EventArgs e)
        {

            SaveFileDialog saver = saveFileDialog1;

            saver.Filter = "Spreadsheet Files (*.ss)|*.ss|All Files|*.*";
            saver.FilterIndex = 1;
            saver.FileName = this.Text;
            saver.OverwritePrompt = true;


            // if file has been saved before, then any further saving will not trigger dialog

            if (saver.ShowDialog() == DialogResult.OK)
            {
                if (saver.FilterIndex == 1)
                    saver.AddExtension = true;
                else
                    saver.AddExtension = false;
                if (FileSaveEvent != null)
                {
                    FileSaveEvent(saver.FileName);
                    Saved = true;
                    IsModified = false;
                }
            }


        }

        /// <summary>
        /// auto-detect if the edit box is click and allow user to update the cell contect accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditBox_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (EnterCellContentEvent != null)
                {

                    EnterCellContentEvent(EditBox.Text);
                    IsModified = true;
                }
                EditBox.Clear();
            }
        }

        /// <summary>
        /// this helper method ensures the update col/row is still within the scope
        /// also update the selected cell in the spreadsheet panel
        /// </summary>
        /// <param name="col_move"></param>
        /// <param name="row_move"></param>
        private void ComputeArrowKeyMovement(int col_move, int row_move)
        {
            int new_col = SelectedCellAddress[0] + col_move;
            int new_row = SelectedCellAddress[1] + row_move;

            if (new_col >= 0 && new_col < 26)
            {
                SelectedCellAddress[0] = SelectedCellAddress[0] + col_move;
            }

            if (new_row >= 0 && new_row < 99)
            {
                SelectedCellAddress[1] = SelectedCellAddress[1] + row_move;
            }

            spreadsheetPanel.SetSelection(SelectedCellAddress[0], SelectedCellAddress[1]);

        }

        /// <summary>
        /// when any ky is pressed, the method will check and move the focused cell according to the arrow key direction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetPanel_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Up)
            {
                ComputeArrowKeyMovement(0, -1);
            }
            else if (e.KeyCode == Keys.Down)
            {
                ComputeArrowKeyMovement(0, 1);
            }
            else if (e.KeyCode == Keys.Left)
            {
                ComputeArrowKeyMovement(-1, 0);
            }
            else if (e.KeyCode == Keys.Right)
            {
                ComputeArrowKeyMovement(1, 0);
            }
            else
            {
                EditBox.Focus();
            }

            spreadsheetPanel.GetSelection(out int col, out int row);
            UpdateTextBoxInfo(col, row);
        }

        /// <summary>
        /// Close method that links to the "close button"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsModified && !Saved)
            {
                if (UnsavedCloseEvent != null)
                {
                    UnsavedCloseEvent(e);
                }
            }

        }

        /// <summary>
        /// close button in the drop down menu, if it's clicked, move on to the file close proccess
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileCloseEvent != null)
            {
                FileCloseEvent();
            }
        }

        /// <summary>
        /// if there is an exception, the program will pop out a message box to warn the user
        /// </summary>
        /// <param name="e"></param>
        public void ShowFileOpenErrorBox(Exception e)
        {
            MessageBox.Show("File could not be read. An exception occured:\n\n"
                                + e.Message, "Unsupported File type");
        }

        /// <summary>
        /// if the program detects the file is changed and have not yet been saved, the program will pop out a messaget to remind users
        /// </summary>
        /// <param name="e"></param>
        public void ShowUnsavedClosePromptBox(FormClosingEventArgs e)
        {
            var window = MessageBox.Show(
                    "Unsaved progress",
                    "Are you sure you want to exit the program?",
                    MessageBoxButtons.YesNo);
            e.Cancel = (window == DialogResult.No);
        }


        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a spreadsheet that contains 26 column and 99 rows. Users are able to" +
                " navigate the workbook by using arrow keys and mouse. To use arrow key movement, please set the spreadsheet panel" +
                " in focus by click on the panel or press TAB key until panel is in focus." +
                " There are two text boxes, namely cell value display and cell content edit." +
                " Users are also able to use formula for any caculation by using reference cells." +
                " Formula must start with '=' to be recognized as a formula; otherwise, it will be treated as a regular string" +
                " Any invalid formula will not be accepted and no change will be made. Any self referencing formula will result it" +
                " a SS.FormulaError value. Please note, to complete a content edit, user must press ENTER to exit edit mode");
        }
    }
}
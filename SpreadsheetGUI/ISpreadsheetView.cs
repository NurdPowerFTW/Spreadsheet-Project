// Created by Wei-Tung, Tang, Chen-Chia Wang
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public interface ISpreadsheetView
    {
        /// <summary>
        /// Fired when a file is chosen with a file dialog.
        /// The parameter is the chosen filename.
        /// </summary>
        event Action<string> FileChosenEvent;

        /// <summary>
        /// event when the user attempt to save the file
        /// </summary>
        event Action<string> FileSaveEvent;

        /// <summary>
        /// event when the user attempt to close the file
        /// </summary>
        event Action FileCloseEvent;

        /// <summary>
        /// event when the program detects the file isn't saved and the user is trying to close the program
        /// </summary>
        event Action<FormClosingEventArgs> UnsavedCloseEvent;
       
        /// <summary>
        /// fired when a new action is requested.
        /// </summary>
        event Action NewEvent;

        /// <summary>
        /// the event when the user is trying to select a cell
        /// </summary>
        event Action<int[]> SelectEvent;

        /// <summary>
        /// the event when user is trying to enter a content into a cell
        /// </summary>
        event Action<string> EnterCellContentEvent;

        /// <summary>
        /// retrun and update the editbox content
        /// </summary>
        int[] SelectedCellAddress { get; set; }

        /// <summary>
        /// update the view box value
        /// </summary>
        string SelectedValue {  set; }

        /// <summary>
        /// retrun and update the editbox content
        /// </summary>
        string SelectedContents { get; set; }


        /// <summary>
        /// set title of the file
        /// </summary>
        string Title { set; }

        /// <summary>
        /// set error message
        /// </summary>
        string Message { set; }

        /// <summary>
        /// Close the file
        /// </summary>
        void DoClose();

        /// <summary>
        /// open a new file
        /// </summary>
        void OpenNew();

        /// <summary>
        /// set value of the focus cell on panel
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        void SetCellValueOnPanel(int[] cell, string value);

        /// <summary>
        /// show the error message box for file opening
        /// </summary>
        /// <param name="e"></param>
        void ShowFileOpenErrorBox(Exception e);

        /// <summary>
        /// pop out a message box to warn user of the unsave file
        /// </summary>
        /// <param name="e"></param>
        void ShowUnsavedClosePromptBox(FormClosingEventArgs e);
    }
}

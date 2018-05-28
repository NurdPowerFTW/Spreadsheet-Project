using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;
using SS;

namespace ControllerTester
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void SetContents1()
        {
            SpreadsheetWindowStub stub = new SpreadsheetWindowStub();

            Controller controller = new Controller(stub, "Nothing");

            stub.ChooseCell(new int[] { 0, 0 });
            stub.PutCellContent("=A10");

            stub.ChooseCell(new int[] { 0, 0 }); 
            Assert.AreEqual("SS.FormulaError", stub.SelectedValueSim);
        }

        [TestMethod]
        public void SetContents2()
        {
            SpreadsheetWindowStub stub = new SpreadsheetWindowStub();

            Controller controller = new Controller(stub, "Nothing");

            stub.ChooseCell(new int[] { 0, 0 });
            stub.PutCellContent("something");

            stub.ChooseCell(new int[] { 0, 0 });
            Assert.AreEqual("something", stub.SelectedValueSim);
        }

        [TestMethod]
        public void SetContents3()
        {
            SpreadsheetWindowStub stub = new SpreadsheetWindowStub();

            Controller controller = new Controller(stub, "Nothing");

            stub.ChooseCell(new int[] { 0, 0 });
            stub.PutCellContent("1");

            stub.ChooseCell(new int[] { 0, 0 });
            Assert.AreEqual("1", stub.SelectedValueSim);
        }

        [TestMethod]
        public void SetContents4()
        {
            SpreadsheetWindowStub stub = new SpreadsheetWindowStub();

            Controller controller = new Controller(stub, "Nothing");

            stub.ChooseCell(new int[] { 0, 0 });
            stub.PutCellContent("=a2 + a3");

            stub.ChooseCell(new int[] { 0, 1 });
            stub.PutCellContent("10");

            stub.ChooseCell(new int[] { 0, 2 });
            stub.PutCellContent("3");

            stub.ChooseCell(new int[] { 0, 0 });
            Assert.AreEqual("13", stub.SelectedValueSim);

        }

        [TestMethod]
        public void CheckContents1()
        {
            SpreadsheetWindowStub stub = new SpreadsheetWindowStub();

            Controller controller = new Controller(stub, "Nothing");

            stub.ChooseCell(new int[] { 1, 1 });
            stub.PutCellContent("b2");

            stub.ChooseCell(new int[] { 1, 1 }); 
            Assert.AreEqual("b2", stub.SelectedContents);
        }

        [TestMethod]
        public void CheckContents2()
        {
            SpreadsheetWindowStub stub = new SpreadsheetWindowStub();

            Controller controller = new Controller(stub, "Nothing");

            stub.ChooseCell(new int[] { 0, 0 });
            stub.PutCellContent("something");

            stub.ChooseCell(new int[] { 0, 0 });
            Assert.AreEqual("something", stub.SelectedContents);
        }

        [TestMethod]
        public void CheckContents3()
        {
            SpreadsheetWindowStub stub = new SpreadsheetWindowStub();

            Controller controller = new Controller(stub, "Nothing");

            stub.ChooseCell(new int[] { 0, 0 });
            stub.PutCellContent("3");

            stub.ChooseCell(new int[] { 0, 0 });
            Assert.AreEqual("3", stub.SelectedContents);
        }


        [TestMethod]
        public void SaveFile1()
        {
            string fileName = @"Spreadsheet1.ss";

            SpreadsheetWindowStub stub = new SpreadsheetWindowStub();

            Controller controller = new Controller(stub, "Nothing");

            stub.PutCellContent("bye");
            stub.ChooseCell(new int[] { 2, 3 });
            stub.PutCellContent("=b2");

            stub.SaveFile(fileName);

            Spreadsheet s;
            using (StreamReader reader = new StreamReader(fileName))
                s = new Spreadsheet(reader, new Regex(@".*"));

            SpreadsheetWindowStub stub2 = new SpreadsheetWindowStub();

            Controller controller2 = new Controller(stub2, s, "Nothing");

            stub2.ChooseCell(new int[] { 0, 0 });
            Assert.AreEqual("bye", stub2.SelectedValueSim);
            stub2.ChooseCell(new int[] { 2, 3 });
            Assert.AreEqual("SS.FormulaError", stub2.SelectedValueSim);
        }

        [TestMethod]
        public void OpenFile()
        {
            SpreadsheetWindowStub stub = new SpreadsheetWindowStub();
            Controller controller = new Controller(stub, "Nothing");
            stub.OpenFile(@"Spreadsheet1.ss");
        }

        [TestMethod]
        public void OpenNewFile()
        {
            SpreadsheetWindowStub stub = new SpreadsheetWindowStub();
            Controller controller = new Controller(stub, "New_spreadsheet");
            stub.OpenNewFile();
            
        }

        [TestMethod]
        public void CloseFile()
        {
            SpreadsheetWindowStub stub = new SpreadsheetWindowStub();
            Controller controller = new Controller(stub, "New_spreadsheet");
            stub.CloseFile();

        }


    }
}

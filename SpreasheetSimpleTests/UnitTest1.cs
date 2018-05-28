using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;

namespace SpreasheetSimpleTests
{
    [TestClass]
    public class UnitTest1
    {
        private AbstractSpreadsheet s = new Spreadsheet();

        [TestMethod]
        public void TestCreateEmptySheet()
        {
            AbstractSpreadsheet s = new Spreadsheet();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestCreateCellWithInvalidName()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("$09", 0);
        }

        [TestMethod]
        public void TestCreateCellWithNumbers1()
        {
            for (int i = 1; i < 10; i++)
            {
                s.SetCellContents("A"+i, i);
            }
            List<object> list = new List<object>();
            foreach(object t in s.GetNamesOfAllNonemptyCells())
            {
                list.Add(t);
            }

            Assert.IsTrue(list.Count == 9);
        }
    }
}

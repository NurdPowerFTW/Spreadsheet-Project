// Unit tests implemented by Wei-Tung Tang Feb-23-2018

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetFinalTests
    {
        /// <summary>
        /// Tests zero-argument constructor
        /// </summary>
        [TestMethod]
        public void SpreadsheetConstructorTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual(false, s.Changed);
        }

        [TestMethod]
        public void SpreadsheetConstructorTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "0");

        }

        /// <summary>
        /// Tests one-argument constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetConstructorTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet(new Regex("^[d-zD-Z]+\\d+$"));
            s.SetContentsOfCell("A1", "0");
        }

        /// <summary>
        /// Tests Save with 
        /// </summary>
        [TestMethod]
        public void TestSave1()
        {
            StringWriter document = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(document))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", "^.*$");

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "a1");
                writer.WriteAttributeString("contents", "CS3500");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet s = new Spreadsheet(new StringReader(document.ToString()), new Regex(""));
            Assert.AreEqual("CS3500", s.GetCellValue("a1").ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestSave2()
        {
            StringWriter document = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(document))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", "\\M");

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "a1");
                writer.WriteAttributeString("contents", "CS3500");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet s = new Spreadsheet(new StringReader(document.ToString()), new Regex(""));
            
        }

        /// <summary>
        /// Tests for duplicated name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestSave3()
        {
            StringWriter document = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(document))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", "^.*$");

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "a1");
                writer.WriteAttributeString("contents", "CS3500");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "a1");
                writer.WriteAttributeString("contents", "CS3505");
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet s = new Spreadsheet(new StringReader(document.ToString()), new Regex(""));
        }

        /// <summary>
        /// Tests for invalid cell name with oldIsValid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestSave4()
        {
            StringWriter document = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(document))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", "^.*$");

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "$1");
                writer.WriteAttributeString("contents", "CS3500");
                writer.WriteEndElement();

            }
            AbstractSpreadsheet s = new Spreadsheet(new StringReader(document.ToString()), new Regex(""));
        }

        /// <summary>
        /// Tests for invalid cell name with newIsValid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetVersionException))]
        public void TestSave5()
        {
            StringWriter document = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(document))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", "^.*$");

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "a1");
                writer.WriteAttributeString("contents", "CS3500");
                writer.WriteEndElement();

            }
            AbstractSpreadsheet s = new Spreadsheet(new StringReader(document.ToString()), new Regex(@"^[b-zB-Z][b-zB-Z]*[1-9][0-9]*$"));
        }

        /// <summary>
        /// Tests for invalid cell content with oldIsValid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestSave6()
        {
            StringWriter document = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(document))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", "^[b-zB-Z][b-zB-Z]*[1-9][0-9]*$");

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "b1");
                writer.WriteAttributeString("contents", "= a1 + a2");
                writer.WriteEndElement();

            }
            AbstractSpreadsheet s = new Spreadsheet(new StringReader(document.ToString()), new Regex(@""));
        }

        /// <summary>
        /// Tests for invalid cell content with newIsValid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetVersionException))]
        public void TestSave7()
        {
            StringWriter document = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(document))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", "^.*$");

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "b1");
                writer.WriteAttributeString("contents", "= a1 + a2");
                writer.WriteEndElement();

            }
            AbstractSpreadsheet s = new Spreadsheet(new StringReader(document.ToString()), new Regex(@"^[b-zB-Z][b-zB-Z]*[1-9][0-9]*$"));
        }

        /// <summary>
        /// Tests for document reading error (attribute cells should be cell)
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestSave8()
        {
            StringWriter document = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(document))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", "^.*$");

                writer.WriteStartElement("cells");
                writer.WriteAttributeString("name", "b1");
                writer.WriteAttributeString("contents", "= a1 + a2");
                writer.WriteEndElement();

            }
            AbstractSpreadsheet s = new Spreadsheet(new StringReader(document.ToString()), new Regex(@"^[b-zB-Z][b-zB-Z]*[1-9][0-9]*$"));
        }

        /// <summary>
        /// Tests for Save function to correctly save content to the document
        /// </summary>
        [TestMethod]
        public void TestSave9()
        {
            StringWriter document = new StringWriter();

            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "CS3500");
            s.Save(document);

            Assert.AreEqual("CS3500", s.GetCellContents("a1"));
        }

        /// <summary>
        /// Tests for Save function to correctly save content to spreadsheet
        /// </summary>
        [TestMethod]
        public void TestSave10()
        {
            StringWriter document = new StringWriter();

            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "= b1 + b2");
            s.Save(document);

            Assert.IsTrue(s.GetCellValue("a1").GetType().Equals(new FormulaError().GetType()));
        }

        /// <summary>
        /// Tests for oldIsValue in the source file is empty
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestSave11()
        {
            StringWriter document = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(document))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", "");

                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", "a1");
                writer.WriteAttributeString("contents", "CS3500");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet s = new Spreadsheet(new StringReader(document.ToString()), new Regex(""));

        }


        /// <summary>
        /// Tests for GetCellContent with non-exisiting cell
        /// </summary>
        [TestMethod]
        public void GetCellContentTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "CS3500");
            
            Assert.AreEqual("", s.GetCellContents("b1"));
        }

        /// <summary>
        /// Tests for GetCellValue with non-exisiting cell
        /// </summary>
        [TestMethod]
        public void GetCellValue1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "CS3500");

            Assert.AreEqual("", s.GetCellValue("b1"));
        }

        /// <summary>
        /// Tests for SetContentsOfCell to correctly update cell and graph dependency
        /// </summary>
        [TestMethod]
        public void SetContentsOfCell1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "= A2 + A3");
            s.SetContentsOfCell("A3", "= B1 + B2");
            s.SetContentsOfCell("B1", "5.0");
            s.SetContentsOfCell("B2", "6.0");
            s.SetContentsOfCell("A2", "9.0");

            Assert.AreEqual(20.0, (double)s.GetCellValue("A1"), 1e-9);

        }

        /// <summary>
        /// Tests for SetContentOfCell to set cell value to FormulaError if undefined variable is evaluated
        /// </summary>
        [TestMethod]
        public void SetContentsOfCell2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "= A2 + A3");
            s.SetContentsOfCell("A3", "= B1 + B2");
            s.SetContentsOfCell("B1", "5.0");
            s.SetContentsOfCell("B2", "6.0");
            s.SetContentsOfCell("A2", "= D3");

            Assert.IsTrue(s.GetCellValue("a1").GetType().Equals(new FormulaError().GetType()));
        }

        /// <summary>
        /// Tests for SetContentOfCell with null content
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetContentsOfCell3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", null);
        }

        /// <summary>
        /// Tests for SetContentOfCell with circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCell4()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1","= A2 + A3");
            s.SetContentsOfCell("A2", "= A1 + A3");
        }

        /// <summary>
        /// Tests for SetContentOfCell with duplicated named cell to correctly update cell content and value
        /// </summary>
        [TestMethod]
        public void SetContentsOfCell5()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "= A2 + A3");
            s.SetContentsOfCell("A1", "= A4 + A5");

            Assert.AreEqual("A4+A5", s.GetCellContents("A1").ToString());
            Assert.IsTrue(s.GetCellValue("A1").GetType().Equals(new FormulaError().GetType()));

        }


        [TestMethod]
        public void TestCreateAHundreadThousandCellsWithNumber()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            for (int i = 1; i < 100001; i++)
            {
                s.SetContentsOfCell("A" + i, i.ToString());
            }
            List<object> list = new List<object>();
            foreach (object t in s.GetNamesOfAllNonemptyCells())
            {
                list.Add(t);
            }
            Assert.AreEqual(100000, list.Count);
        }

        /// <summary>
        /// Stress test to see if a random cell content is consistent
        /// </summary>
        [TestMethod]
        public void StressTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            List<string> list = new List<string>();

            for (int i = 1; i < 300; i++)
            {
                s.SetContentsOfCell("A" + i, i.ToString());
            }
            
            for (int j = 1; j < 300; j++)
            {
                list.Add(j.ToString());
            }

            Assert.AreEqual(list[149], s.GetCellContents("A150").ToString());
        }
    }
}

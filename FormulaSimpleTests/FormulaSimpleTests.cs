// Written by Joe Zachary for CS 3500, January 2017.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    { 
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate0()
        {
            Formula f = new Formula("1+2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 6, 1e-6);
        }
        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2.5+3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.5, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5 ()
        {
            Formula f = new Formula("25 + 3");
            Assert.AreEqual(f.Evaluate(Lookup4), 28, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate6()
        {
            Formula f = new Formula("(5-1) / x");
            Assert.AreEqual(f.Evaluate(Lookup4), (5-1)/4.0, 1e-6);
        }


        /// <summary>
        /// This tests Evaluate with a normalized variables 
        /// </summary>
        [TestMethod]
        public void Evaluate7()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6", x => x.ToUpper(), x => true);
            Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9)   ;
        }

        /// <summary>
        /// This tests 1-arugment constructor to see if it throws an ArgumentNullException if 
        /// a null string parameter is passed in
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest1()
        {
            Formula f = new Formula(null);
        }

        /// <summary>
        /// This tests 3-arugments constructor to see if it throws an ArgumentNullException if 
        /// a null string parameter is passed in
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest2()
        {
            Formula f = new Formula(null, s => s, s=>true);
        }

        /// <summary>
        /// This tests 3-arugments constructor to see if it throws an ArgumentNullException if 
        /// a null Normalizer or Validator is passed in
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest3()
        {
            Formula f = new Formula("x+y", null, null);
        }

        /// <summary>
        /// This tests Evaluate() to throw exception when a null lookup is passed in
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest4()
        {
            Formula f = new Formula("x+y");
            double temp = f.Evaluate(null);
        }

        /// <summary>
        /// This tests behavior invoked by default construtor with ToString()
        /// </summary>
        [TestMethod]
        public void TestOnDefaultContrusctor1()
        {
            Formula f = new Formula();
            string expected = "0";
            string actual = f.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests behavior invoked by default construtor with Evaluate()
        /// </summary>
        [TestMethod]
        public void TestOnDefaultContrusctor2()
        {
            Formula f = new Formula();
            Assert.AreEqual(0.0, f.Evaluate(s => 0), 1e-6);
        }

        /// <summary>
        /// This tests Formula constructor with all variables normalized to upper case and
        /// and an additional rule imposed by validator
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNormalizer0()
        {
            string s = "x1+y22";
            Regex constraint = new Regex( @"^[a-zA-Z][0-9]$");
            Formula f = new Formula(s, x => x.ToUpper(), x => constraint.IsMatch(x));
            
        }

        /// <summary>
        /// This test Formula constructor with all variables normalized to lower case
        /// </summary>
        [TestMethod]
        public void TestNormalizer1()
        {
            string s = "X1+Y2";
            Regex constraint = new Regex(@"^[a-zA-Z][0-9]$");
            Formula f = new Formula(s, x => x.ToLower(), x => constraint.IsMatch(x));
            string actual = f.ToString();
            string expected = "x1+y2";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests if two Formula objects behave in the same way in terms of their
        /// ToString() result
        /// </summary>
        [TestMethod]
        public void TestToString1()
        {
            Formula f1 = new Formula("x+y");
            Formula f2 = new Formula(f1.ToString(), s => s, s => true);
            string expected = f1.ToString();
            string actual = f2.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests a Formula object created by default constructor should behave
        /// the same way as the one created by 1-arg constructor
        /// </summary>
        [TestMethod]
        public void TestToString3()
        {
            Formula f1 = new Formula();
            Formula f2 = new Formula("0");
            string expected = f2.ToString();
            string actual = f1.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests GetVariable() returns correct normalized variables
        /// </summary>
        [TestMethod]
        public void TestGetVariables1()
        {
            Regex constraint = new Regex(@"^[a-zA-Z][0-9]$");
            Formula f = new Formula("(x1+y2)+(5/1)", x => x.ToUpper(), x => constraint.IsMatch(x));
            List<string> list = new List<string>
            {
                "X1",
                "Y2"
            };

            int i = 0;
            foreach(string token in f.GetVariables())
            {
                Assert.AreEqual(list[i], token);
                i++;
            }
        }

        /// <summary>
        /// This tests GetVariable() returns correct number of normalized variables if duplicated 
        /// variables are present in the list.
        /// </summary>
        [TestMethod]
        public void TestGetVariables2()
        {
            Regex constraint = new Regex(@"^[a-zA-Z][0-9]$");
            Formula f = new Formula("(x1+x1+x1)+(5/1)", x => x.ToUpper(), x => constraint.IsMatch(x));

            int expected = 1;
            int actual = f.GetVariables().Count;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests GetVariable() returns correct number of normalized variables if duplicated 
        /// variables are present in the list by using Formula created with 1-argument constructor 
        /// </summary>
        [TestMethod]
        public void TestGetVariables3()
        {
            Regex constraint = new Regex(@"^[a-zA-Z][0-9]$");
            Formula f = new Formula("(x1+x1+x1)+(5/1)");

            int expected = 1;
            int actual = f.GetVariables().Count;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This test behavior of GetVariables when using an object created by default constructor to invoke the method
        /// </summary>
        [TestMethod]
        public void TestGetVariables4()
        {
            Formula f = new Formula();
            f.GetVariables();
        }

        [TestMethod]
        public void TestGetVariables5()
        {
            Formula f = new Formula("12+5");
            ISet<string> list = f.GetVariables();
            int expected = 0;
            int actual = list.Count;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }
    }
}

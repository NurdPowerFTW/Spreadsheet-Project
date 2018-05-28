// Test implementation written by Wei-Tung Tang for CS 3500, January 2018.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyGraphTest
{
    /// <summary>
    /// This test class tests public methods in DependencyGraph class.
    /// Each method is covered with basic and advanced tests. 
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// This tests a empty dependency graph.
        /// </summary>
        [TestMethod]
        public void TestZeroDependency()
        {
            DependencyGraph graph = new DependencyGraph();
            int expected = 0;
            int actual = graph.Size;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSecondConstructorOnNullParam()
        {
            DependencyGraph graph = new DependencyGraph(null);
        }

        /// <summary>
        /// This tests Size to report correct number.
        /// </summary>
        [TestMethod]
        public void TestSize()
        {
            DependencyGraph graph = new DependencyGraph();
            for (int i = 0; i < 10; i++)
            {
                graph.AddDependency(i.ToString(), (i + 1).ToString());
            }
            int expected = 10;
            int actual = graph.Size;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests HasDependent with a null parameter.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestHasDependentsOnNullParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.HasDependents(null);
        }

        /// <summary>
        /// This tests HasDependents with a non-exisiting entry.
        /// </summary>
        [TestMethod]
        public void TestHasDependentsOnInvalidParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            bool expected = false;
            bool actual = graph.HasDependents("C");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests HasDependents with entry that reports correct result
        /// if it has dependent(s).
        /// </summary>
        [TestMethod]
        public void TestHasDependentsOnValidParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            bool expected = true;
            bool actual = graph.HasDependents("A");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This test HasDependee with a null parameter.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestHasDependeeOnNullParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            graph.HasDependees(null);
        }

        /// <summary>
        /// This tests HasDependee with a non-existing entry.
        /// </summary>
        [TestMethod]
        public void TestHasDependeeOnInvalidParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            bool expected = false;
            bool actual = graph.HasDependees("C");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests HasDependee with entry that reports correct result
        /// if it has dependee(s).
        /// </summary>
        [TestMethod]
        public void TestHasDependeeOnValidParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            bool expected = true;
            bool actual = graph.HasDependees("B");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetDependentsOnNullParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            List<string> list = graph.GetDependents(null).ToList();
        }

        /// <summary>
        /// This tests GetDependents with non-existing entry
        /// </summary>
        [TestMethod]
        public void TestGetDependentsOnInvalidParam()
        {
            DependencyGraph graph = new DependencyGraph();

            for (int i = 0; i < 10; i++)
            {
                graph.AddDependency("A", i.ToString());

            }

            int expected = 0;
            List<string> list = new List<string>();
            list = graph.GetDependents("C").ToList();
            int actual = list.Count;

            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// This tests GetDependents with entry that has a non-empty
        /// set.
        /// </summary>
        [TestMethod]
        public void TestGetDependentsOnValidParam()
        {
            DependencyGraph graph = new DependencyGraph();

            for (int i = 0; i < 10; i++)
            {
                graph.AddDependency("A", i.ToString());

            }

            List<string> expected = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                expected.Add(i.ToString());
            }

            List<string> actual = new List<string>();

            actual = graph.GetDependents("A").ToList();

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetDependeesOnNullParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            List<string> list = graph.GetDependees(null).ToList();
        }

        /// <summary>
        /// This tests GetDependees with entry that has a non-empty
        /// set.
        /// </summary>
        [TestMethod]
        public void TestGetDependeesOnValidParam()
        {
            DependencyGraph graph = new DependencyGraph();

            for (int i = 0; i < 10; i++)
            {
                graph.AddDependency(i.ToString(), "A");

            }

            int expectedNum = 0;
            foreach (string actual in graph.GetDependees("A"))
            {
                string expected = expectedNum.ToString();
                expectedNum++;
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// This tests AddDependency with 100_000 dependencies.
        /// Execution time must be lower than 1 second.
        /// </summary>
        [TestMethod]
        public void TestAddHundreadThousandDependencies()
        {
            DependencyGraph graph = new DependencyGraph();
            Stopwatch sw = new Stopwatch();
            Random rand = new Random();

            sw.Start();
            for (int i = 0; i < 100_000; i++)
            {
                int first = rand.Next(0, 10_000);
                int second = rand.Next(0, 10_000);
                graph.AddDependency(first.ToString(), second.ToString());
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Assert.IsTrue(ts.Seconds <= 1);
        }

        [TestMethod]
        public void TestAddHundreadThousandDependenciesUsingSecondConstructor()
        {
            DependencyGraph graph1 = new DependencyGraph();
            Stopwatch sw = new Stopwatch();
            Random rand = new Random();

            for (int i = 0; i < 100_000; i++)
            {
                int first = rand.Next(0, 10_000);
                int second = rand.Next(0, 10_000);
                graph1.AddDependency(first.ToString(), second.ToString());
            }

            sw.Start();
            DependencyGraph graph2 = new DependencyGraph(graph1);
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Assert.IsTrue(ts.Seconds <= 1);
        }
        /// <summary>
        /// This tests AddDependency with duplicated entries.
        /// </summary>
        [TestMethod]
        public void TestAddDependencyOnDuplicateEntries()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            graph.AddDependency("A", "B");
            graph.AddDependency("A", "B");
            graph.AddDependency("A", "B");
            graph.AddDependency("A", "C");
            // expected depdency number
            int expected = 2;
            int actual = graph.Size;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests AddDependency with both null parameters.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddDependencyOnNullEntries()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(null, null);

        }

        /// <summary>
        /// This tests RemoveDependency with both null parameters.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveDependencyOnNullParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            graph.RemoveDependency(null, null);

        }

        /// <summary>
        /// This tests RemoveDependency with non-exisiting dependency
        /// </summary>
        [TestMethod]
        public void TestRemoveDependcyOnInvalidParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            graph.AddDependency("A", "C");
            graph.RemoveDependency("A", "D");

            int expected = 2;
            int actual = graph.Size;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This test RemoveDependency with exisiting parameter.
        /// </summary>
        [TestMethod]
        public void TestRemoveDependcyOnValidParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            graph.RemoveDependency("A", "B");

            int expected = 0;
            int actual = graph.Size;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests ReplaceDependents with null parameters.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependentsOnNullParam1()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            List<string> list = new List<string>()
            {
                "A",
                "B",
                null
            };
            graph.ReplaceDependents(null, list);

        }

        /// <summary>
        /// This tests ReplaceDependents with null parameters.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependentsOnNullParam2()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            List<string> list = new List<string>()
            {
                "A",
                "B",
                null
            };
            graph.ReplaceDependents("A", list);

        }

        /// <summary>
        /// This tests ReplaceDependents with invalid param.
        /// </summary>
        [TestMethod]
        public void TestReplaceDependentsOnInvalidParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            graph.AddDependency("A", "C");
            graph.AddDependency("C", "B");
            graph.AddDependency("C", "D");

            var list = new List<string>
            {
                "A",
                "B"
            };

            graph.ReplaceDependents("A", list);

            int expected = 4;
            int actual = graph.Size;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests ReplaceDependents with valid paramters.
        /// The graph should report correct dependents as end result.
        /// </summary>
        [TestMethod]
        public void TestReplaceDependentsOnValidParam()
        {
            DependencyGraph graph = new DependencyGraph();
            for (int i = 0; i < 10; i++)
            {
                graph.AddDependency("A", i.ToString());
            }

            List<string> list = new List<string>();
            for (int j = 0; j < 10; j++)
            {
                list.Add((j * 2).ToString());
            }

            graph.ReplaceDependents("A", list);

            int k = 0;
            // Check if the entry hold the correct dpendents set after the replacement
            foreach (string dependent in graph.GetDependents("A"))
            {
                string expected = (k * 2).ToString();
                string actual = dependent;
                Assert.AreEqual(expected, actual);
                k++;
            }
        }

        /// <summary>
        /// This tests ReplaceDependees with both parameters are null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependeesOnNullParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");

            List<string> list = new List<string>()
            {
                "A",
                null,
                "B"
            };
            graph.ReplaceDependees(null, list);
        }

        /// <summary>
        /// This tests ReplaceDependees with a null element inside of newDependee
        /// list. It should throw ArgumentNullException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependeesOnInvalidParam()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("A", "B");
            graph.AddDependency("C", "B");
            graph.AddDependency("D", "B");
            graph.AddDependency("E", "C");

            var list = new List<string>
            {
                "E",
                null,
                "F"
            };

            graph.ReplaceDependees("B", list);

            int expected = 3;
            int actual = graph.Size;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests ReplaceDependees with valid parameters. 
        /// </summary>
        [TestMethod]
        public void TestReplaceDependeesOnValidParam()
        {
            DependencyGraph graph = new DependencyGraph();
            for (int i = 0; i < 2; i++)
            {
                graph.AddDependency(i.ToString(), "A");
            }

            List<string> list = new List<string>();
            for (int j = 0; j < 2; j++)
            {
                list.Add((j * 2).ToString());
            }

            graph.ReplaceDependees("A", list);

            int k = 0;
            // Examine each dependee has correct value
            foreach (string dependee in graph.GetDependees("A"))
            {
                string expected = (k * 2).ToString();
                string actual = dependee;
                Assert.AreEqual(expected, actual);
                k++;
            }
        }

        /// <summary>
        /// This tests adding 100_000 dependencies and removing 50_000 ones that would
        /// still report the correct size. 
        /// </summary>
        [TestMethod]
        public void StressTestOnRemoveFiftyThousandDependcies()
        {
            DependencyGraph graph = new DependencyGraph();

            for (int i = 0; i < 100_000; i++)
            {
                graph.AddDependency(i.ToString(), (i + 1).ToString());
            }

            for (int j = 0; j < 50_000; j++)
            {
                graph.RemoveDependency(j.ToString(), (j + 1).ToString());
            }
            int expected = 50_000;
            int actual = graph.Size;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests removing dependency right after adding one that would still
        /// give the correct size
        /// </summary>
        [TestMethod]
        public void StressTestOnAddAndRemoveMultipleDepencies()
        {
            DependencyGraph graph = new DependencyGraph();

            for (int i = 0; i < 100_000; i++)
            {
                graph.AddDependency(i.ToString(), (i + 1).ToString());
                graph.RemoveDependency(i.ToString(), (i + 1).ToString());
            }

            int expected = 0;
            int actual = graph.Size;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This tests if ReplaceDependents yields the correct graph after 
        /// all the dependees are replaced.
        /// </summary>
        [TestMethod]
        public void StressTestReplaceDependentsHundreadThousandTimes()
        {
            DependencyGraph graph = new DependencyGraph();

            for (int i = 0; i < 100_000; i++)
            {
                graph.AddDependency("A", (i + 1).ToString());
            }

            List<string> list = new List<string>();
            for (int j = 0; j < 100_000; j++)
            {
                list.Add((j * 5).ToString());
            }

            graph.ReplaceDependents("A", list);

            int k = 0;
            foreach (string dependent in graph.GetDependents("A"))
            {
                Assert.AreEqual(list[k++], dependent);
            }
        }

        /// <summary>
        /// This tests if ReplaceDependees yields the correct graph after 
        /// all the dependees are replaced.
        /// </summary>
        [TestMethod]
        public void StressTestReplaceDependeesHundreadThousandTimes()
        {
            DependencyGraph graph = new DependencyGraph();

            for (int i = 0; i < 100_000; i++)
            {
                graph.AddDependency(i.ToString(), "A");
            }

            List<string> list = new List<string>();
            for (int j = 0; j < 100_000; j++)
            {
                list.Add((j * 5).ToString());
            }

            graph.ReplaceDependees("A", list);

            int k = 0;
            foreach (string dependee in graph.GetDependees("A"))
            {
                Assert.AreEqual(list[k++], dependee);
            }
        }

        /// <summary>
        /// This tests a mix of ReplaceDepents and ReplaceDependees to see
        /// if the depependGraph still contains consistent dependents and 
        /// dependees after the mixed operations.
        /// </summary>
        [TestMethod]
        public void MixedTestOnReplaceDependentsAndDependees()
        {
            DependencyGraph graph = new DependencyGraph();

            for (int i = 0; i < 1000; i++)
            {
                graph.AddDependency("A", i.ToString());
            }

            for (int j = 0; j < 1000; j++)
            {
                graph.AddDependency("B", j.ToString());
            }

            for (int k = 0; k < 1000; k++)
            {
                graph.AddDependency(k.ToString(), "C");
            }

            List<string> list1 = new List<string>();
            for (int m = 0; m < 1999; m++)
            {
                list1.Add(m.ToString());
            }

            graph.ReplaceDependees("A", list1);

            List<string> list2 = new List<string>();
            // Use character from (lowercase) a through z as new dependees
            for (int unicode = 97; unicode < 123; unicode++)
            {
                char character = (char)unicode;
                list2.Add(character.ToString());
            }
            graph.ReplaceDependees("C", list2);

            int index = 0;
            foreach (string dependent in graph.GetDependents("A"))
            {
                Assert.AreEqual(list1[index++], dependent);
            }

            index = 0;

            foreach (string dependee in graph.GetDependees("C"))
            {
                Assert.AreEqual(list2[index++], dependee);
            }

            index = 0;
            foreach (string dependent in graph.GetDependents("B"))
            {
                Assert.AreEqual((index++).ToString(), dependent);
            }
        }

        /// <summary>
        /// This test the secondary constructor to generate a distinct graph object
        /// </summary>
        [TestMethod]
        public void TestNewConstructor()
        {
            DependencyGraph graph1 = new DependencyGraph();
            graph1.AddDependency("A", "B");
            graph1.AddDependency("A", "C");
            DependencyGraph graph2 = new DependencyGraph(graph1);
            graph2.AddDependency("F", "C");

            Assert.AreNotEqual(graph1.Size, graph2.Size);

        }

        // ********************************** A STESS TEST, REPEATED ******************** //
        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        public void StressTestOnSecondaryConstructor1()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 800;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Make a new graph t2 out of t
            DependencyGraph t2 = new DependencyGraph(t);

            // Remove a bunch of dependencies from t2
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 3)
                {
                    t2.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependees from t2
            for (int i = 0; i < SIZE; i += 2)
            {
                HashSet<string> newDees = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 9)
                {
                    newDees.Add(letters[j]);
                }
                t2.ReplaceDependees(letters[i], newDees);

                foreach (string s in dees[i])
                {
                    dents[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDees)
                {
                    dents[s[0] - 'a'].Add(letters[i]);
                }

                dees[i] = newDees;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t2.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t2.GetDependees(letters[i]))));
            }
        }

        [TestMethod()]
        public void StressTestOnSecondaryConstructor2()
        {
            StressTestOnSecondaryConstructor1();
        }
        [TestMethod()]
        public void StressTestOnSecondaryConstructor3()
        {
            StressTestOnSecondaryConstructor1();
        }
        [TestMethod()]
        public void StressTestOnSecondaryConstructor4()
        {
            StressTestOnSecondaryConstructor1();
        }
        [TestMethod()]
        public void StressTestOnSecondaryConstructor5()
        {
            StressTestOnSecondaryConstructor1();
        }

    }
    
}
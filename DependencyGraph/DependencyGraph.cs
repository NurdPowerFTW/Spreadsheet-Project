// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.
// Solution implementation written by Wei-Tung Tang for CS 3500, January 2018.
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Used to store all the dependents information for look-up, access, and modification
        /// </summary>
        private Dictionary<string, HashSet<string>> dependentDict;

        /// <summary>
        /// used to store all the dependees information for look-up, access, and modification
        /// </summary>
        private Dictionary<string, HashSet<string>> dependeeDict;

        /// <summary>
        /// A counter to keep track of current number of exisiting dependency
        /// </summary>
        private int depedencySize;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            // Initialize all instance variables
            dependentDict = new Dictionary<string, HashSet<string>>();
            dependeeDict = new Dictionary<string, HashSet<string>>();
            depedencySize = 0;

        }

        /// <summary>
        /// This constructor creates a constructor by copying from another graph object.
        /// The graph object generated from this constructor should be independent from its
        /// original copy.
        /// </summary>
        public DependencyGraph(DependencyGraph graph)
            : this()
        {
            if (graph == null)
            {
                throw new ArgumentNullException();
            }

            foreach (KeyValuePair<string, HashSet<string>> entry in graph.dependentDict)
            {
                foreach (string dependent in entry.Value)
                {
                    AddDependency(entry.Key, dependent);
                }
            }
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            { return depedencySize; }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.  If s is null, throw ArgumentNullException
        /// </summary>
        public bool HasDependents(string s)
        {
            // If s is null, return false to signify caller that look-up fails
            if (s == null)
                throw new ArgumentNullException();

            if (dependentDict.ContainsKey(s))
            {
                if (dependentDict[s].Count > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  If s is null, throw ArgumentNullException
        /// </summary>
        public bool HasDependees(string s)
        {
            // If s is null, return false to signify caller that look-up fails
            if (s == null)
                throw new ArgumentNullException();

            if (dependeeDict.ContainsKey(s))
            {
                if (dependeeDict[s].Count > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  If s is null, throw ArgumentNullException
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }

            if (dependentDict.ContainsKey(s))
            {
                // Use yield return to generate dependent set as they are requested by the 
                // consumer.
                foreach (string key in dependentDict[s])
                {
                    yield return key;
                }
            }
        }


        /// <summary>
        /// Enumerates dependees(s).  If s is null, throw ArgumentNullException
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }

            if (dependeeDict.ContainsKey(s))
            {
                // Use yield return to generate dependee set as they are requested by the 
                // consumer.
                foreach (string key in dependeeDict[s])
                {
                    yield return key;
                }
            }
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// If s or t is null, throw ArgumentNullException
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                HashSet<string> dependentSet = new HashSet<string>();
                HashSet<string> dependeeSet = new HashSet<string>();


                if (dependentDict.Count < 1)
                {
                    // Add first key in depdents dictionary and first value in the hashset
                    dependentSet.Add(t);
                    dependentDict.Add(s, dependentSet);
                    // Update dependee dictionary and set as well
                    dependeeSet.Add(s);
                    dependeeDict.Add(t, dependeeSet);
                    depedencySize++;
                 }
                else
                {
                    // Check if a key already exists in the dictionary
                    if (dependentDict.ContainsKey(s))
                    {
                        // If an exisiting (s, t) dependency is deteced, there is no effect 
                        if (!dependentDict[s].Contains(t))
                        {
                            dependentDict[s].Add(t);
                            UpdateDependeeList(s, t);
                            depedencySize++;
                        }
                    }
                    else
                    {
                        // If not, create a new key 
                        HashSet<string> newSet = new HashSet<string> { t };

                        dependentDict.Add(s, newSet);
                        UpdateDependeeList(s, t);
                        depedencySize++;
                    }
                }
            }

        }

        /// <summary>
        /// This private helper is called from AddDependency to update dependee information.
        /// </summary>
        private void UpdateDependeeList(string s, string t)
        {
            if (dependeeDict.ContainsKey(t))
            {
                dependeeDict[t].Add(s);
            }
            else
            {
                HashSet<string> newDependeeSet = new HashSet<string> { s };
                dependeeDict.Add(t, newDependeeSet);
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// If s or t is null, throw ArgumentNullException.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                if (dependentDict.ContainsKey(s))
                {
                    if (dependentDict[s].Contains(t))
                    {
                        dependentDict[s].Remove(t);
                        // If key s has no more dependent, remove it from the dictionary
                        // a empty key floating around in the dictionary is not allowed.
                        if (!HasDependents(s))
                        {
                            dependentDict.Remove(s);
                        }

                        dependeeDict[t].Remove(s);
                        // If key t has no more dependee, remove it from the dictionary
                        // a empty key floating around in the dictionary is not allowed.
                        if (!HasDependees(t))
                        {
                            dependeeDict.Remove(t);
                        }
                        depedencySize--;
                    }
                }

            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// If s or t is null, throw ArgumentNullException
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s != null)
            {

                // Since alternating the hashSet while iterating through it is not allowed,
                // creates a copy of such list and use it to traverse the set.
                List<string> tempList = new List<string>();
                foreach (string value in GetDependents(s))
                {
                    tempList.Add(value);
                }

                foreach (string dependent in tempList)
                {
                    RemoveDependency(s, dependent);
                }

                // Add the dependency with corresponding value from newDependents
                foreach (string t in newDependents)
                {
                    // If t is null, then skip this step
                    if (t != null)
                    {
                        AddDependency(s, t);
                    }
                    else
                    {
                        throw new ArgumentNullException();
                    }
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// If s or t is null, throw ArgumentNullException
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t != null)
            {
                // Get a copy of dependees and use it to traverse through the entire set
                List<string> tempList = new List<string>();

                foreach (string value in GetDependees(t))
                {
                    tempList.Add(value);
                }

                foreach (string dependee in tempList)
                {
                    RemoveDependency(dependee, t);
                }

                // Add the dependency with corresponding value from newDependees
                foreach (string s in newDependees)
                {
                    // If s is null, skip this step
                    if (s != null)
                    {
                        AddDependency(s, t);
                    }
                    else
                    {
                        throw new ArgumentNullException();
                    }

                }

            }
            else
            {
                throw new ArgumentNullException();
            }

        }
    }


}
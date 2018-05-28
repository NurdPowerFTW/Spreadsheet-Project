// Created and Implemented by Wei-Tung Tang Feb-23-2018

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{
    /// <summary>
    /// This class stores information about Cell name and content
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// A unique cell identifier
        /// </summary>
        private string name;

        /// <summary>
        /// This contains cell's content. It can be (1) a string, (2) a double, or (3) a Formula.
        /// </summary>
        private object content;

        private object value;

        /// <summary>
        /// Setter and getter for name
        /// </summary>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// Setter and getter for content
        /// </summary>
        public object Content { get => content; set => this.content = value; }

        /// <summary>
        /// Setter and getter for value
        /// </summary>
        public object Value { get => value; set => this.value = value; }
    }
}

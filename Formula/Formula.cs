// Skeleton written by Joe Zachary for CS 3500, January 2017
// Solution written by Wei-Tung Tang for CS 3500, January 2018

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public struct Formula
    {
        /// <summary>
        /// Used to store tokens that passed the constructor validation check
        /// </summary>
        private List<string> stringList;
        
        /// <summary>
        /// Create a new Formula contructor which takes three arugments. It takes an additional Normalizer
        /// to normalize all variables in the list and an additional Validator to impose additional 
        /// restriction on the variable format.
        /// </summary>
        public Formula(string formula, Normalizer normalizer, Validator validator)
        {
            if (formula == null)
                throw new ArgumentNullException();

            if (normalizer == null || validator == null)
                throw new ArgumentNullException();

            stringList = new List<string>();
            IEnumerable<string> list = GetTokens(formula);
            // If GetToken return empty result, throw exception
            if (list.Count() == 0)
            {
                throw new FormulaFormatException("This is no token");
            }
            // Variables to keep track of open/close parentheses 
            int opCount = 0;
            int cpCount = 0;

            // Variable to keep track of previous taken
            string preToken = "";

            Regex varRegex = new Regex(@"[a-zA-Z][0-9a-zA-Z]*");
            Regex opRegex = new Regex(@"^[\+\-*/]$");

            // Check if first token is valid
            var first = list.First();
            if (!first.Equals("(") && !varRegex.IsMatch(first) && !Double.TryParse(first, out double firstValue))
            {
                throw new FormulaFormatException("First token Invalid");
            }

            // Check if last token is valid
            var last = list.Last();
            if (!last.Equals(")") && !varRegex.IsMatch(last) && !Double.TryParse(last, out double lastValue))
            {
                throw new FormulaFormatException("Last token Invalid");
            }

            // check for each preToken for different scenario 
            foreach (string token in list)
            {
               
                // if there is more than one token in the list
                if (!preToken.Equals(""))
                {
                    // Check case 7 in which any token immediately follows a opening parenthese(op) or an operator
                    // must be either a num, a var, or an op.
                    if (preToken.Equals("(") || opRegex.IsMatch(preToken))
                    {

                        if (!Double.TryParse(token, out double parsedValue) && !varRegex.IsMatch(token) && !token.Equals("("))
                        {
                            throw new FormulaFormatException("Invalid token: " + token + " captured");
                        }

                    }
                    // Check case 8 in which any token immediately follows a num, a var, or a closing parenthese (cp)
                    // must be either an op or a cp.
                    else if (preToken.Equals(")") || Double.TryParse(preToken, out double parsedValue) || varRegex.IsMatch(preToken))
                    {

                        if (!opRegex.IsMatch(token) && !token.Equals(")"))
                        {
                            throw new FormulaFormatException("Invalid token: " + token + " captured!");
                        }
                    }
                }
                preToken = token;

                if (token.Equals("("))
                {
                    opCount++;
                }
                if (token.Equals(")"))
                {
                    cpCount++;
                }

                if (cpCount > opCount)
                    throw new FormulaFormatException("Closing parentheses number does not match opening ones");
            }

            if (opCount != cpCount)
                throw new FormulaFormatException("Closing parentheses number does not match opening ones");

            stringList = list.ToList();

            // When all tokens are checked for validation, perform Normalization 
            for (int tokenIndex = 0; tokenIndex < stringList.Count; tokenIndex++)
            {

                string examined = stringList[tokenIndex];
                if (varRegex.IsMatch(examined))
                {
                    string normalized = normalizer(examined);
                    bool result = validator(normalized);
                    // If a token is still a valid variable after normalization and pass the validator
                    // test, then add it to the list; otherwise, throw FormulaFormatException
                    if (varRegex.IsMatch(normalized) && result)
                    {
                        stringList[tokenIndex] = normalized;
                    }
                    else
                    {
                        throw new FormulaFormatException("Invalid token: " + normalized + " captured");
                    }
                }
            }
        }
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// This constructor invokes the 3-arugments constructor. If this constructor is called, default normalier and
        /// validator value are passed.
        /// </summary>
        public Formula(string formula)
            :this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// This method return a set of normalized variables.
        /// </summary>
        public ISet<string> GetVariables()
        {
            // return an empty set in case default constructor is invoked
            if (stringList == null)
            {
                return new HashSet<string>();
            }

            ISet<string> set = new HashSet<string>();
            Regex varRegex = new Regex(@"[a-zA-Z][0-9a-zA-Z]*");

            foreach (string token in stringList)
            {
                if (varRegex.IsMatch(token))
                {
                    set.Add(token);
                }
            }
            return set;

        }

        /// <summary>
        /// This overrides the default ToString method by returning a string that is consisted of 
        /// normalized tokens.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // In the event of default constructor being called, simply return 0
            if (stringList == null)
            {
                return "0";
            }

            string temp = "";

            foreach(string token in stringList)
            {
                temp += token;
            }

            return temp;
        }

       

        /// <summary>
        /// A Lookup method is one that maps some strings to double values.  Given a string,
        /// such a function can either return a double (meaning that the string maps to the
        /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
        /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
        /// don't is up to the implementation of the method.
        /// </summary>
        public delegate double Lookup(string var);

        /// <summary>
        /// A Normalizer method used to normalized any passed-in token. Given a token "X2", if normalization
        /// converts the string into lower case, then the token will be "x2".
        /// </summary>
        public delegate string Normalizer(string s);

        /// <summary>
        /// A Validator method used to impose additional validation restriction on the normalized tokens
        /// </summary>
        public delegate bool Validator(string s);

        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            if (lookup == null)
            {
                throw new ArgumentNullException();
            }

            // In the event in which default constructor is called, simply return 0.0
            if (stringList == null)
            {
                return 0.0;
            }


            // Declare stacks for values and operators
            Stack<double> valueStack = new Stack<double>();
            Stack<string> opStack = new Stack<string>();

            Regex varRegex = new Regex(@"[a-zA-Z][0-9a-zA-Z]*");

            foreach (string token in stringList)
            {
                double result = 0.0;

                // Check if current toke is parsable (an int or a double)
                if (Double.TryParse(token, out double parsedValue))
                {
                    double second = parsedValue;
                    // If operator stack is not empty, there must be a prior value stored in value stack
                    // multiplcation or division must be performed if either one of them exists
                    if (opStack.Count > 0)
                    {
                        if (opStack.Peek().Equals("*") || opStack.Peek().Equals("/"))
                        {
                            if (valueStack.Count > 0)
                            {
                                // Value that is on top of value stack
                                double first = valueStack.Pop();
                                switch (opStack.Pop())
                                {
                                    case "*":
                                        result = first * second;
                                        break;
                                    case "/":
                                        if (second == 0)
                                            throw new FormulaEvaluationException("Zero denominator is not allowed");
                                        result = first / second;
                                        break;

                                }
                                valueStack.Push(result);
                            }
                        }
                        else
                        {
                            // If there is no available operator, just store the current value token
                            valueStack.Push(second);
                        }
                    }
                    else
                    {
                        valueStack.Push(second);
                    }

                }
                // Check if current token is a variable
                else if (varRegex.IsMatch(token))
                {
                    double converted;
                    // If current token is a variable, attempt to map value to that variable
                    try
                    {
                        converted = lookup(token);
                    }
                    catch (UndefinedVariableException)
                    {
                        throw new FormulaEvaluationException("Failed to map value to a variable");
                    }
                    if (opStack.Count > 0)
                    {
                        if (opStack.Peek().Equals("*") || opStack.Peek().Equals("/"))
                        {
                            if (valueStack.Count > 0)
                            {
                                double first = valueStack.Pop();
                                double second = converted;
                                switch (opStack.Pop())
                                {
                                    case "*":
                                        result = first * second;
                                        break;
                                    case "/":
                                        if (second == 0)
                                            throw new FormulaEvaluationException("Zero denominator is not allowed");
                                        result = first / second;
                                        break;
                                }
                                valueStack.Push(result);
                            }
                        }
                        else
                        {
                            valueStack.Push(converted);
                        }
                    }
                    else
                    {
                        valueStack.Push(converted);
                    }
                }
                // If current token is an operator
                else if (token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/"))
                {
                    // If + or - is on top of opStack, compute the result for the 2 values storing in
                    // value stack and push it to value stack
                    if (token.Equals("+") || token.Equals("-"))
                    {
                        if (opStack.Count > 0)
                        {
                            if (opStack.Peek().Equals("+") || opStack.Peek().Equals("-"))
                            {
                                if (valueStack.Count > 1)
                                {
                                    double second = valueStack.Pop();
                                    double first = valueStack.Pop();

                                    switch (opStack.Pop())
                                    {
                                        case "+":
                                            result = first + second;
                                            break;
                                        case "-":
                                            result = first - second;
                                            break;
                                    }
                                    valueStack.Push(result);
                                }
                            }
                        }
                    }

                    // Otherwise, just push the operator onto stack
                    opStack.Push(token);
                }
                else if (token.Equals("("))
                {
                    opStack.Push("(");
                }
                else
                {
                    // If current token is ")", and + or - is on top of the operator stack, pop the value stack
                    // twice and perform the operation then push the result to the value stack
                    if (opStack.Count > 0)
                    {
                        if (opStack.Peek().Equals("+") || opStack.Peek().Equals("-"))
                        {
                            if (valueStack.Count > 1)
                            {
                                double second = valueStack.Pop();
                                double first = valueStack.Pop();

                                switch (opStack.Pop())
                                {
                                    case "+":
                                        result = first + second;
                                        break;
                                    case "-":
                                        result = first - second;
                                        break;
                                }
                                valueStack.Push(result);
                            }
                        }
                    }
                    // Pop out the opening parenthese on top of operator stack
                    opStack.Pop();

                    // If * or / is on top of operator stack, pop the value stack twice and the operator once
                    // to get the result of these two values. Then push it onto the value stack.
                    if (opStack.Count > 0)
                    {
                        if (opStack.Peek().Equals("*") || opStack.Peek().Equals("/"))
                        {
                            if (valueStack.Count > 0)
                            {
                                double second = valueStack.Pop();
                                double first = valueStack.Pop();

                                switch (opStack.Pop())
                                {
                                    case "*":
                                        result = first * second;
                                        break;
                                    case "/":
                                        if (second == 0)
                                            throw new FormulaEvaluationException("Zero denominator is not allowed");
                                        result = first / second;
                                        break;
                                }
                                valueStack.Push(result);
                            }
                        }
                    }

                }
            }

            // If operator stack is empty, just return the only value inside of value stack.
            if (opStack.Count == 0)
            {
                return valueStack.Pop();
            }
            // Otherwise, there will exactly 2 values left in the stack. And the only operator left will be
            // + or -. Perform the operation and return the result.
            else
            {
                double result = 0;
                double second = valueStack.Pop();
                double first = valueStack.Pop();
                switch (opStack.Pop())
                {
                    case "+":
                        result = first + second;
                        break;
                    case "-":
                        result = first - second;
                        break;
                }
                return result;
            }

        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
           
            // Patterns for individual tokens.
            // NOTE:  These patterns are designed to be used to create a pattern to split a string into tokens.
            // For example, the opPattern will match any string that contains an operator symbol, such as
            // "abc+def".  If you want to use one of these patterns to match an entire string (e.g., make it so
            // the opPattern will match "+" but not "abc+def", you need to add ^ to the beginning of the pattern
            // and $ to the end (e.g., opPattern would need to be @"^[\+\-*/]$".)
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

            // PLEASE NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.  This pattern is useful for 
            // splitting a string into tokens.
            String splittingPattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            // PLEASE NOTE:  Notice the second parameter to Split, which says to ignore embedded white space
            /// in the pattern.
            foreach (String s in Regex.Split(formula, splittingPattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }



    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    [Serializable]
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}

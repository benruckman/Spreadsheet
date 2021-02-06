// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        /// 

        private List<string> Variables;

        private String NormalizedFormula;

        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            Variables = new List<string>();
            Double currentDouble;
            int numOpenParenthese = 0;
            int numClosedParentese = 0;

            String prevToken = "";

            foreach (String currentToken in GetTokens(formula))
            {
                //beggining 
                if (prevToken == "(" || IsOperator(prevToken))
                {
                    if (currentToken == ")" || IsOperator(currentToken))
                    {
                        throw new FormulaFormatException("Invalid character following ( or operator");
                    }
                }

                if (prevToken == ")" || Double.TryParse(prevToken, out _) || IsVariable(prevToken, normalize, isValid))
                {
                    if (!(IsOperator(currentToken) || currentToken == ")"))
                    {
                        throw new FormulaFormatException("Invalid character following ) or Variable or Double");
                    }
                }

                //handles when Double appears as current token
                if (Double.TryParse(currentToken, out currentDouble))
                {
                    NormalizedFormula += currentDouble.ToString();
                }

                //handles when operator appears as current token
                else if (IsOperator(currentToken))
                {
                    if (prevToken == "")
                    {
                        throw new FormulaFormatException("Operator cannot appear as first character");
                    }

                    NormalizedFormula += currentToken;
                }

                //handles when ( appears as current token
                else if (currentToken == "(")
                {
                    numOpenParenthese++;
                    NormalizedFormula += currentToken;
                }

                //handles when ) appears as current token
                else if (currentToken == ")")
                {
                    numClosedParentese++;
                    if (numClosedParentese > numOpenParenthese)
                    {
                        throw new FormulaFormatException("Invalid arrangment of parentheses");
                    }

                    NormalizedFormula += currentToken;
                }

                //handles when variable appears as current token
                else if (IsVariable(currentToken, normalize, isValid))
                {
                    NormalizedFormula += normalize(currentToken);
                    if (!Variables.Contains(normalize(currentToken)))
                    {
                        Variables.Add(normalize(currentToken));
                    }

                }

                //handles when an invalid token appears as current token
                else
                {
                    throw new FormulaFormatException("Invalid character appeared");
                }

                prevToken = currentToken;
            }

            if (prevToken == "")
            {
                throw new FormulaFormatException("Empty String appeared");
            }

            if (numClosedParentese != numOpenParenthese)
            {
                throw new FormulaFormatException("Must be same number of open and closed parentheses");
            }

            if (IsOperator(prevToken) || prevToken == "(")
            {
                throw new FormulaFormatException("Formula cannot end with an operator or (");
            }
        }

        /// <summary>
        /// helper method for determing if the given string is a variable
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool IsVariable(String s, Func<string, string> normalize, Func<string, bool> isValid)
        {
            return Regex.IsMatch(s, "^[a-zA-Z_]([a-zA-Z_]|\\d)*$") && isValid(normalize(s));
        }

        /// <summary>
        /// helper method for determining if current string is an operator
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool IsOperator(String s)
        {
            if (s == "+" || s == "-" || s == "*" || s == "/")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            try
            {
                return EvaluateHelper(NormalizedFormula, lookup);
            }
            catch
            {
                return new FormulaError("Divide By Zero Occured");
            }
        }

        /// <summary>
        /// Helper Method for Evaluating Expression, this is the code copied and modified from PS1
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="lookup"></param>
        /// <returns></returns>
        private static Double EvaluateHelper(string exp, Func<string, double> lookup)
        {
            //Seperate String into Substrings (Code from PS1
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //Declare Value and Operator Stack
            Stack<Double> valueStack = new Stack<Double>();
            Stack<String> operatorStack = new Stack<String>();

            //Iterate through tokens in substring
            String currentToken;
            foreach (String t in substrings)
            {
                currentToken = t.Trim();


                //If currentToken is an integer
                if (Double.TryParse(currentToken, out _))
                {
                    DoubleHandler(operatorStack, valueStack, Double.Parse(currentToken));
                }
                //If currentToken is a variable
                else if (Regex.IsMatch(currentToken, "^[a-zA-Z]+[0-9]+$"))
                {
                    DoubleHandler(operatorStack, valueStack, lookup(currentToken));
                }
                //If currentToken is a + or -
                else if (currentToken == "+" || currentToken == "-")
                {
                    AdditionSubtractionHandler(operatorStack, valueStack, currentToken);
                }
                //If currentToken is * or /
                else if (currentToken == "*" | currentToken == "/")
                {
                    operatorStack.Push(currentToken);
                }
                //If currentToken is a (
                else if (currentToken == "(")
                {
                    operatorStack.Push(currentToken);
                }
                //If currentToken is a )
                else if (currentToken == ")")
                {
                    ClosedParenthesesHandler(operatorStack, valueStack);
                }
            }

            //After every token has been gone through
            if (operatorStack.Count() == 0)
            {
                return valueStack.Pop();
            }
            else
            {
                Double term2 = valueStack.Pop();
                Double term1 = valueStack.Pop();
                return SimpleExpressionSolver(term1, term2, operatorStack.Pop());
            }
        }

        /*
         * Solves arithmetic expressions with two values and one operator
         * term1: left term in expression
         * term2: right term in expression
         * operation: String of operation to be applied
         * throws argument expression
         */
        private static Double SimpleExpressionSolver(Double term1, Double term2, String operation)
        {
            if (operation == "+")
            {
                return term1 + term2;
            }
            else if (operation == "-")
            {
                return term1 - term2;
            }
            else if (operation == "*")
            {
                return term1 * term2;
            }
            else
            {
                if (term2 == 0)
                {
                    throw new ArgumentException("Invalid Expression cant divide by 0");
                }
                return term1 / term2;
            }
        }

        /*
         * Handles algorithm when current token is an integer
         * operatorStack: reference to stack containing all of the seen operators in expression
         * valueStack: reference to stack containg all of the seen values in expression
         * currentToken
         */
        private static void DoubleHandler(Stack<String> operatorStack, Stack<Double> valueStack, Double currentToken)
        {
            if (operatorStack.Count() > 0 && (operatorStack.Peek() == "/" | operatorStack.Peek() == "*"))
            {

                valueStack.Push(SimpleExpressionSolver(valueStack.Pop(), currentToken, operatorStack.Pop()));
            }
            else
            {
                valueStack.Push(currentToken);
            }
        }

        /*
         * Handles algorithm when current token is either + or -
         * operatorStack: reference to stack containing all of the seen operators in expression
         * valueStack: reference to stack containg all of the seen values in expression
         * currentToken
         */
        private static void AdditionSubtractionHandler(Stack<String> operatorStack, Stack<Double> valueStack, String currentToken)
        {
            if (operatorStack.Count() != 0 && (operatorStack.Peek() == "+" | operatorStack.Peek() == "-"))
            {
                //pop'd in this order to ensure left to right order is maintened
                Double term2 = valueStack.Pop();
                Double term1 = valueStack.Pop();
                valueStack.Push(SimpleExpressionSolver(term1, term2, operatorStack.Pop()));
            }

            operatorStack.Push(currentToken);
        }

        private static void ClosedParenthesesHandler(Stack<String> operatorStack, Stack<Double> valueStack)
        {
            if (operatorStack.Count() > 0 && operatorStack.Peek() == "+" | operatorStack.Peek() == "-")
            {
                //pop'd in this order to ensure left to right order is maintened
                Double term2 = valueStack.Pop();
                Double term1 = valueStack.Pop();
                valueStack.Push(SimpleExpressionSolver(term1, term2, operatorStack.Pop()));
            }

            operatorStack.Pop();

            if (operatorStack.Count() > 0 && (operatorStack.Peek() == "*" | operatorStack.Peek() == "/"))
            {
                Double term2 = valueStack.Pop();
                Double term1 = valueStack.Pop();
                valueStack.Push(SimpleExpressionSolver(term1, term2, operatorStack.Pop()));
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return Variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return NormalizedFormula;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return NormalizedFormula.Equals(obj.ToString());
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return Equals(f1, f2) && f1.ToString().Equals(f2.ToString());
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return NormalizedFormula.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace FormulaEvaluator
{
    public static class Evaluator
    {
        public delegate double Lookup(string v);

        /*
         * Method for evaluating intger arithmetic expressions
         * 
         */
        public static Double Evaluate(String exp, Lookup variableEvaluator)
        {
            //Seperate String into Substrings (Code from PS1
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //Declare Value and Operator Stack
            Stack<Double> valueStack = new Stack<Double>();
            Stack<String> operatorStack = new Stack<String>();

            //Iterate through tokens in substring
            String currentToken;
            foreach(String t in substrings)
            {
                currentToken = t.Trim();

                if (!Regex.IsMatch(currentToken, "^[a-zA-Z_]+[0-9]+$")
                    && !Double.TryParse(currentToken, out _)
                    && currentToken != "*" && currentToken != "/" && currentToken != "+" 
                    && currentToken != "-" && currentToken != "(" && currentToken != ")" && currentToken != "")
                {
                    throw new ArgumentException("Invalid Character in Expression");
                }
                else if (substrings.Count() == 1 && currentToken == "")
                {
                    throw new ArgumentException("Empty String");
                }

                //If currentToken is an integer
                if(Double.TryParse(currentToken, out _))
                {
                    DoubleHandler(operatorStack, valueStack, int.Parse(currentToken));
                }
                //If currentToken is a variable
                else if(Regex.IsMatch(currentToken, "^[a-zA-Z]+[0-9]+$"))
                {
                    DoubleHandler(operatorStack, valueStack, variableEvaluator(currentToken));
                }
                //If currentToken is a + or -
                else if(currentToken == "+" || currentToken == "-")
                {
                    AdditionSubtractionHandler(operatorStack, valueStack, currentToken);
                }
                //If currentToken is * or /
                else if(currentToken == "*" | currentToken == "/")
                {
                    operatorStack.Push(currentToken);
                }
                //If currentToken is a (
                else if(currentToken == "(")
                {
                    operatorStack.Push(currentToken);
                }
                //If currentToken is a )
                else if(currentToken == ")")
                {
                    ClosedParenthesesHandler(operatorStack, valueStack);
                }
            }

            //After every token has been gone through
            if(operatorStack.Count() == 0)
            {
                return valueStack.Pop();
            }
            else if (operatorStack.Count() == 1 && valueStack.Count() == 2)
            {
                Double term2 = valueStack.Pop();
                Double term1 = valueStack.Pop();
                return SimpleExpressionSolver(term1, term2, operatorStack.Pop());
            }
            else
            {
                throw new ArgumentException("Invalid Formula Entered");
            }
        }

        /*
         * Solves arithmetic expressions with two values and one operator
         * term1: left term in expression
         * term2: right term in expression
         * operation: String of operation to be applied
         * throws argument expression
         */
        private static Double SimpleExpressionSolver (Double term1, Double term2, String operation)
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
            else if (operation == "/")
            {
                if (term2 == 0)
                {
                    throw new ArgumentException("Invalid Expression cant divide by 0");
                }
                return term1 / term2;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /*
         * Handles algorithm when current token is an integer
         * operatorStack: reference to stack containing all of the seen operators in expression
         * valueStack: reference to stack containg all of the seen values in expression
         * currentToken
         */
        private static void DoubleHandler (Stack<String> operatorStack, Stack<Double> valueStack, Double currentToken)
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
            if(operatorStack.Count() > 0 && operatorStack.Peek() == "+" | operatorStack.Peek() == "-")
            {
                //pop'd in this order to ensure left to right order is maintened
                Double term2 = valueStack.Pop();
                Double term1 = valueStack.Pop();
                valueStack.Push(SimpleExpressionSolver(term1, term2, operatorStack.Pop()));
            }

            //Operator should be (
            if (operatorStack.Count == 0 || operatorStack.Pop() != "(")
            {
                throw new ArgumentException("Error, ( parenthese did not appear when it should have");
            }

            if(operatorStack.Count() > 0 && (operatorStack.Peek() == "*" | operatorStack.Peek() == "/"))
            {
                Double term2 = valueStack.Pop();
                Double term1 = valueStack.Pop();
                valueStack.Push(SimpleExpressionSolver(term1, term2, operatorStack.Pop()));
            }
        }

        //public static object Evaluate(string normalizedFormula, Func<string, double> lookup)
        //{
        //    throw new NotImplementedException();
        //}
    }

   
   
}

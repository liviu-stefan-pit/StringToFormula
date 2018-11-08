using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringToFormulaTest
{
    public class StringToFormula
    {
        #region Fields
        private string[] _operators = { "-", "+", "*", "/", "^"};
        private Func<double, double, double>[] _operations =
        {
            (a, b) => a - b,
            (a, b) => a + b,
            (a, b) => a * b,
            (a, b) => a / b,
            (a, b) => Math.Pow(a, b)
        };
        #endregion

        #region Public Methods
        public double EvaluateExpression(string expression)
        {
            List<string> tokens = GetTokens(expression);
            Stack<double> operandsStack = new Stack<double>();
            Stack<string> operatorsStack = new Stack<string>();
            int tokenIndex = 0;

            while(tokenIndex < tokens.Count)
            {
                string token = tokens[tokenIndex];

                if (token == "(")
                {
                    string subExpression = GetSubExpression(tokens, ref tokenIndex);
                    operandsStack.Push(EvaluateExpression(subExpression));
                    continue;
                }
                if(token == ")")
                    throw new ArgumentException("Check your expression for mis-matched parentheses.");

                if(Array.IndexOf(_operators, token) >= 0)
                {
                    while(operatorsStack.Count > 0 && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorsStack.Peek()))
                    {
                        string op = operatorsStack.Pop();
                        double arg2 = operandsStack.Pop();
                        double arg1 = operandsStack.Pop();
                        operandsStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                    }
                    operatorsStack.Push(token);
                }
                else
                {
                    operandsStack.Push(double.Parse(token));
                }
                tokenIndex += 1;
            }

            while(operatorsStack.Count > 0)
            {
                string op = operatorsStack.Pop();
                double arg2 = operandsStack.Pop();
                double arg1 = operandsStack.Pop();
                operandsStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
            }

            return operandsStack.Pop();
        }
        #endregion

        #region Private Methods
        private List<string> GetTokens(string expression)
        {
            string operators = "()^*/+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach(char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if (sb.Length > 0)
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                    sb.Append(c);
            }

            if (sb.Length > 0)
                tokens.Add(sb.ToString());

            return tokens;
        }

        private string GetSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpression = new StringBuilder();
            int levels = 1;
            index += 1;

            while(index <= tokens.Count && levels > 0)
            {
                string token = tokens[index];

                if (tokens[index] == "(")
                    levels += 1;

                if (tokens[index] == ")")
                    levels -= 2;

                if (levels > 0)
                    subExpression.Append(token);

                index += 1;
            }

            if (levels > 0)
                throw new ArgumentException("Check your expression for mis-matched parentheses.");

            return subExpression.ToString();
        }
        #endregion
    }
}

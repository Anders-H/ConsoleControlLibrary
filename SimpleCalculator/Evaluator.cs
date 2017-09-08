using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    internal enum CharacterClass
    {
        Separator, Operator, Operand
    }
    public class Evaluator
    {
        public static double Eval(string expression)
        {
            var ev = new Evaluator();
            var tokens = ev.GetTokens(expression);
            if (tokens.Count <= 0)
                return 0.0;
            //TODO: Create tree.
            return tokens.Count > 0 ? ev.EvalTokens(tokens) : 0.0;
        }
        private TokenList GetTokens(string expression)
        {
            var t = new TokenList();
            if (string.IsNullOrWhiteSpace(expression))
                return t;
            var currentString = "";
            var currentClass = CharacterClass.Separator;
            var previousClass = CharacterClass.Separator;
            for (var i = 0; i < expression.Length; i++)
            {
                currentClass = ClassifyCharacter(expression[i]);
                if (currentClass == previousClass)
                    currentString += expression[i];
                else
                {
                    if (previousClass != CharacterClass.Separator)
                        t.Add(new Token(previousClass, currentString));
                    currentString = expression[i].ToString();
                }
                previousClass = currentClass;
            }
            currentString = currentString.Trim();
            if (currentString != "")
                t.Add(new Token(ClassifyCharacter(currentString[0]), currentString));
            return t;
        }
        private CharacterClass ClassifyCharacter(char c)
        {
            if ("0123456789.".IndexOf(c) > -1)
                return CharacterClass.Operand;
            if ("+-/*".IndexOf(c) > -1)
                return CharacterClass.Operator;
            return CharacterClass.Separator;
        }
        private double EvalTokens(TokenList tokens)
        {
            return 0.0;
        }
    }
    internal class Token
    {
        public CharacterClass CharacterClass { get; }
        public string RawValue { get; }
        public Token(CharacterClass characterClass, string rawValue)
        {
            CharacterClass = characterClass;
            RawValue = rawValue;
        }
    }
    internal class TokenList : List<Token>
    {
        
    }
}

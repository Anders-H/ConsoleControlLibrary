using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
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
            if (!tokens.Last().IsOperand)
                throw new SystemException("Must end with operator.");
            var inputTokens = new TokenList();
            tokens.ForEach(x => inputTokens.Add(x));
            var pass1 = new TokenList();
            bool iterateHighPriorityAgain;
            do
            {
                iterateHighPriorityAgain = false;
                Token previous = null;
                foreach (var t in inputTokens)
                {
                    if (t.AccountedFor || previous == null)
                    {
                        if (!t.AccountedFor)
                            pass1.Add(t);
                        previous = t;
                        continue;
                    }
                    if (t.IsHighPriorityOperator && !previous.IsOperand)
                        throw new SystemException("Expected operand.");
                    if (t.IsHighPriorityOperator && previous.IsOperand)
                    {
                        var nextIndex = inputTokens.IndexOf(t) + 1;
                        if (nextIndex >= inputTokens.Count)
                            throw new SystemException("Unexpected end.");
                        var next = inputTokens[nextIndex];
                        if (!next.IsOperand)
                            throw new SystemException("Expected operand.");
                        switch (t.OperatorValue)
                        {
                            case "*":
                                pass1.Add(new Token(CharacterClass.Operand, previous.OperandValue * next.OperandValue));
                                break;
                            case "/":
                                pass1.Add(new Token(CharacterClass.Operand, previous.OperandValue / next.OperandValue));
                                break;
                            default:
                                throw new SystemException($"Unexpected high operator: {t.OperatorValue}");
                        }
                        previous.AccountedFor = true;
                        t.AccountedFor = true;
                        next.AccountedFor = true;
                        iterateHighPriorityAgain = true;
                        pass1.Remove(previous);
                        //Recreate and restart.
                        if (inputTokens.Count > nextIndex + 1)
                            for (var i = 0; i < inputTokens.Count - 1; i++)
                                pass1.Add(inputTokens[i]);
                        inputTokens.Clear();
                        foreach (var x in pass1)
                        {
                            x.AccountedFor = false;
                            inputTokens.Add(x);
                        }
                        break;
                    }
                    if (!t.AccountedFor)
                        pass1.Add(t);
                    previous = t;
                }

            } while (iterateHighPriorityAgain);
            bool removeAgain;
            do
            {
                removeAgain = false;
                var accountedToken = pass1.FirstOrDefault(x => x.AccountedFor);
                if (accountedToken == null)
                    continue;
                pass1.Remove(accountedToken);
                removeAgain = true;
            } while (removeAgain);
            Debug.WriteLine("Pass 1:");
            foreach (var t in pass1)
                Debug.WriteLine(t);
            return 0.0;
        }
    }
    internal class Token
    {
        public bool AccountedFor { get; set; }
        public CharacterClass CharacterClass { get; }
        public string RawValue { get; }
        private double? Value { get; } = null;
        public Token(CharacterClass characterClass, string rawValue)
        {
            CharacterClass = characterClass;
            RawValue = (rawValue ?? "").Trim();
        }
        public Token(CharacterClass characterClass, double value)
        {
            CharacterClass = characterClass;
            Value = value;
        }
        public string OperatorValue => IsOperator ? RawValue : "";
        public double OperandValue
        {
            get
            {
                if (Value.HasValue)
                    return Value.Value;
                if (double.TryParse(RawValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double s))
                    return s;
                throw new SystemException($"Failed to parse operand: {RawValue}");
            }
        }
        public bool IsHighPriorityOperator => CharacterClass == CharacterClass.Operator && (RawValue == "*" || RawValue == "/");
        public bool IsLowPriorityOperator => CharacterClass == CharacterClass.Operator && (RawValue == "+" || RawValue == "-");
        public bool IsOperator => CharacterClass == CharacterClass.Operator;
        public bool IsOperand => CharacterClass == CharacterClass.Operand;
        public override string ToString() => $"{CharacterClass}: {(string.IsNullOrEmpty(RawValue) && Value.HasValue ? Value.Value.ToString("n4") : RawValue)}";
    }
    internal class TokenList : List<Token>
    {
    }
}

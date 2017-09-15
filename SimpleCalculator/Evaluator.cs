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
            var engine = new Jint.Engine();
            engine.Execute(expression);
            var x = engine.GetCompletionValue();
            return x.AsNumber();
        }
    }
}

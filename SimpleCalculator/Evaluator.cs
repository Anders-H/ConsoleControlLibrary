namespace SimpleCalculator
{
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

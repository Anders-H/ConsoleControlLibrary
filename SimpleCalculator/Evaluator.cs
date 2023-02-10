namespace SimpleCalculator;

public class Evaluator
{
    private static readonly Jint.Engine Engine;

    static Evaluator()
    {
        Engine = new Jint.Engine();
    }

    public static double Eval(string expression)
    {
        Engine.Execute(expression);
        var x = Engine.GetCompletionValue();
        return x.AsNumber();
    }
}
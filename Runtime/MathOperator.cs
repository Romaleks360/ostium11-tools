namespace Ostium11
{
    public enum MathOperator
    {
        Set,
        Add,
        Subtract,
        Multiply,
        Divide,
    }

    public static class MathOperatorExtensions
    {
        public static float Evaluate(this MathOperator op, float lhs, float rhs) => op switch
        {
            MathOperator.Set => rhs,
            MathOperator.Subtract => lhs - rhs,
            MathOperator.Add => lhs + rhs,
            MathOperator.Multiply => lhs * rhs,
            MathOperator.Divide => lhs / rhs,
            _ => lhs,
        };
    }
}

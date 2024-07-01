using System;

namespace Ostium11
{
    [Flags]
    public enum Comparison
    {
        Less = 1,
        Equal = 2,
        Greater = 4,
        LessOrEqual = Less | Equal,
        NotEqual = Less | Greater,
        GreaterOrEqual = Greater | Equal,
    }

    public static class ComparisonExtensions
    {
        public static bool Compare(this Comparison comparison, float val1, float val2) =>
            ((int)comparison & (1 << (Math.Sign(val1.CompareTo(val2)) + 1))) != 0;

        public static bool Compare(this Comparison comparison, int val1, int val2) =>
            ((int)comparison & (1 << (Math.Sign(val1.CompareTo(val2)) + 1))) != 0;
        
        public static string Sign(this Comparison comparison) => comparison switch
        {
            Comparison.Less => "<",
            Comparison.Equal => "=",
            Comparison.Greater => ">",
            Comparison.LessOrEqual => "<=",
            Comparison.NotEqual => "!=",
            Comparison.GreaterOrEqual => ">=",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
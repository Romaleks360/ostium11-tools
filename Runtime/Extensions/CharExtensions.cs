namespace Ostium11.Extensions
{
    public static class CharExtensions
    {
        public static bool IsTrueDigit(this char c) => c >= '0' && c <= '9';
    }
}
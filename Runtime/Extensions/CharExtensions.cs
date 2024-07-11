namespace Ostium11.Extensions
{
    public static class CharExtensions
    {
        public static bool IsDigit(this char c) => c >= '0' && c <= '9';

        public static int GetDigit(this char c) => c - '0';
    }
}
using System;
using System.Collections.Generic;

namespace Ostium11.Extensions
{
    public static class StackExtensions
    {
        public static T PopOrCreate<T>(this Stack<T> stack) where T : new()
        {
            if (stack.Count == 0)
                return new();
            return stack.Pop();
        }

        public static T PopOrCreate<T>(this Stack<T> stack, T defaultValue)
        {
            if (stack.Count == 0)
                return defaultValue;
            return stack.Pop();
        }

        public static T PopOrCreate<T>(this Stack<T> stack, Func<T> create)
        {
            if (stack.Count == 0)
                return create();
            return stack.Pop();
        }
    }
}
using System.Collections.Generic;

namespace Ostium11
{
    public class FixedSizeStack<T>
    {
        private readonly LinkedList<T> _list;

        public int Size { get; set; }

        public int Count => _list.Count;

        public FixedSizeStack(int size)
        {
            _list = new LinkedList<T>();
            Size = size;
        }

        public void Push(T obj)
        {
            _list.AddFirst(obj);
            while (_list.Count > Size)
                _list.RemoveLast();
        }

        public T Pop()
        {
            var obj = _list.First.Value;
            _list.RemoveFirst();
            return obj;
        }

        public T Peek() => _list.First.Value;

        public void Clear() => _list.Clear();
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Dersei.Doto.Collections
{
    public class RandomCollection<T>
    {
        private readonly List<T> _list = new List<T>();
        private readonly List<T> _removed = new List<T>();
        public int Count => _list.Count;
        public int UniqueCount => _uniqueCount;
        private int _uniqueCount;
        
        public RandomCollection(params (T item, int count)[] items)
        {
            foreach (var (item, count) in items)
            {
                if (!_list.Contains(item))
                {
                    _uniqueCount++;
                }
                for (var i = 0; i < count; i++)
                {
                    _list.Add(item);
                }
            }
        }

        public void Add(T item, int count)
        {
            if (!_list.Contains(item))
            {
                _uniqueCount++;
            }
            for (var i = 0; i < count; i++)
            {
                _list.Add(item);
            }
        }

        public void Remove(T item, int count)
        {
            for (var i = 0; i < count; i++)
            {
                if (!_list.Remove(item)) return;
            }
            if (!_list.Contains(item))
            {
                _uniqueCount--;
            }
        }

        public T GetRandom()
        {
            if (_list.Count == 0)
            {
                if (_removed.Count == 0) return default;
                _list.AddRange(_removed);
            }
            if (_uniqueCount == 1)
            {
                var lastItem = _list[0];
                _list.Remove(lastItem);
                _removed.Add(lastItem);
                return _list[0];
            }
            var index = Random.Range(0, _list.Count);
            var item = _list[index];
            _list.Remove(item);
            _removed.Add(item);
            return item;
        }
    }
}

using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class ListExtensions
    {
        public static T GetRandomElement<T>(this List<T> list, bool doRemoveElement = false)
        {
            var elementsAmount = list.Count;
            if (elementsAmount == 0) throw new ArgumentException("List has 0 elements");
            var random = new Random();
            var index = random.Next(elementsAmount);
            var element = list[index];
            if (doRemoveElement) list.RemoveAt(index);
            return element;
        }
    }
}

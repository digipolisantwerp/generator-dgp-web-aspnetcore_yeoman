using System;
using System.Collections.Generic;
using System.Linq;

namespace Digipolis.Utilities
{
    public class ListHelper
    {
        public static void RemoveTypes<TType>(IList<TType> list, Type type)
        {
            var objects = list.Where(e => e.GetType().IsAssignableFrom(type) ).ToList();
            foreach (var item in objects)
            {
                list.Remove(item);
            }
        }
    }
}

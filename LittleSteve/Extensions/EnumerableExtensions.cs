using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleSteve.Extensions
{
    public static class EnumerableExtensions
    {
        //https://stackoverflow.com/questions/3453274/using-linq-to-get-the-last-n-elements-of-a-collection
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int n)
        {
            var enumerable = source.ToList();
            return enumerable.Skip(Math.Max(0, enumerable.Count() - n));
        }
    }
}

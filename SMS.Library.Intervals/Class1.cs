using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5.Intervals;

namespace SMS.Library.Intervals
{
    public class ExtendedDoublyLinkedFiniteIntervalTree<I, T> : DoublyLinkedFiniteIntervalTree<I, T>
        where I : class, IInterval<T>
        where T : IComparable<T>
    {
        public void ForceAdd(I interval, Action<I, I> action)
        {
            // Add

            //var node = _root;
        }
    }
}

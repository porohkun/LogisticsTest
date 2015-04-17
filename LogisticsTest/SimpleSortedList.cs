using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogisticsTest
{
    public class SimpleSortedList : List<PathNode>
    {
        public new void Add(PathNode node)
        {
            if (Count == 0 || node.CompareTo(this[Count - 1]) <= 0)
            {
                base.Add(node);
                return;
            }
            if (node.Fuel >= this[0].Fuel)
            {
                Insert(0, node);
                return;
            }

            int index = FindNodeWithSameFuel(node.Fuel);
            if (index < 0)
            {
                Insert(index, node);
            }
            else if (this[index].City != node.City)
            {
                Insert(index, node);
            }
        }

        public PathNode Pop()
        {
            PathNode result = this[Count - 1];
            RemoveAt(Count - 1);
            return result;
        }

        private int FindNodeWithSameFuel(double Score)
        {
            int low = 0;
            int high = Count - 1;

            while (low <= high)
            {
                int i = low + ((high - low) >> 1);

                double s = this[i].Fuel;
                if (s > Score)
                {
                    low = i + 1;
                }
                else if (s < Score)
                {
                    high = i - 1;
                }
                else
                {
                    return i;
                }
            }

            return low;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LogisticsTest
{
    [DebuggerDisplay("{Fuel}, {City.Name}")]
    public class PathNode
    {
        public readonly PathNode Parent;
        public readonly City City;
        public readonly double Fuel;

        public PathNode(City city, PathNode parent, double distance)
        {
            this.City = city;
            this.Parent = parent;
            this.Fuel = distance;
        }

        public int CompareTo(PathNode other)
        {
            return Fuel.CompareTo(other.Fuel);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LogisticsTest
{
    [DebuggerDisplay("{Name}")]
    public class City
    {
        public string Name;
        public Dictionary<City, double> Roads;

        public City(string name)
        {
            Name = name;
            Roads = new Dictionary<City, double>();
        }

        public void AddRoad(City c1, double fuel)
        {
            if (Roads.ContainsKey(c1))
                Roads[c1] = Math.Min(Roads[c1], fuel);
            else
                Roads.Add(c1, fuel);
        }
    }
}

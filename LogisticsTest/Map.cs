using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogisticsTest
{
    public class Map
    {
        public Dictionary<string, City> Cities = new Dictionary<string, City>();

        public Map() { }
        
        public City AddCity(string cityName)
        {
            if (!Cities.ContainsKey(cityName))
            {
                City city = new City(cityName);
                Cities.Add(cityName, city);
                return city;
            }
            return GetCity(cityName);
        }

        public City GetCity(string cityName)
        {
            return Cities[cityName];
        }

        public void AddRoad(string city1, string city2, double fuel)
        {
            City c1 = AddCity(city1);
            City c2 = AddCity(city2);
            c1.AddRoad(c2, fuel);
            c2.AddRoad(c1, fuel);
        }

    }
}

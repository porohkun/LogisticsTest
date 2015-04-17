using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogisticsTest
{
    public static class Logistics
    {
        public static List<City> CalculatePath(City start, City target)
        {
            if (start == target)
            {
                return new List<City> { start };
            }

            bool starting = true;

            SimpleSortedList openNodes = new SimpleSortedList();
            List<City> closedNodes = new List<City>();
            SimpleSortedList pathes = new SimpleSortedList();

            PathNode currentNode = new PathNode(start, null, 0);

            do
            {
                if (!starting)
                    currentNode = openNodes.Pop();
                starting = false;
                City currentCity = currentNode.City;

                if (closedNodes.Contains(currentCity))
                    continue;

                if (currentCity == target)
                {
                    //path found
                    pathes.Add(currentNode);
                    continue;
                }
                else
                    closedNodes.Add(currentCity);

                foreach (City direction in currentCity.Roads.Keys)
                {
                    if (closedNodes.Contains(direction))
                        continue;

                    double fuel = currentNode.Fuel + currentCity.Roads[direction];
                    openNodes.Add(new PathNode(direction, currentNode, fuel));
                }

            } while (openNodes.Count > 0);

            if (pathes.Count == 0)
                return new List<City>();

            Func<PathNode, List<City>, List<City>> rec = null;
            rec = (node,path) =>
            {
                path.Add(node.City);
                if (node.Parent != null)
                    return rec(node.Parent, path);
                path.Reverse();
                return path;
            };

            return rec(pathes.Pop(), new List<City>());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogisticsTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Map cityMap = new Map();
            City start;
            City target;

            try
            {

                string[] lines = System.IO.File.ReadAllLines("input.txt");
                string[] line0 = lines[0].Split(' ');
                line0 = line0.Where(item => item != "").ToArray();

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] line = lines[i].Split(' ');
                    line = line.Where(item => item != "").ToArray();
                    if (line.Length == 3)
                        cityMap.AddRoad(line[0], line[1], double.Parse(line[2]));
                }

                start = cityMap.Cities[line0[0]];
                target = cityMap.Cities[line0[1]];
            }
            catch 
            {
                Console.WriteLine("parsing error");
                Console.ReadLine();
                return;
            }

            List<City> path = Logistics.CalculatePath(start, target);

            switch (path.Count)
            {
                case 0:
                    Console.WriteLine("path not found");
                    System.IO.File.WriteAllText("output.txt", "-1");
                    break;
                case 1:
                    Console.WriteLine("start city == target city");
                    System.IO.File.WriteAllText("output.txt", "0");
                    break;
                default:
                    int tab = 0;
                    foreach (City city in path)
                        tab = Math.Max(tab, city.Name.Length);
                    tab += 3;
                    double total = 0;
                    int tabLength = 0;
                    Console.WriteLine(path[0].Name.PadRight(tab) + total);
                    for (int i = 1; i < path.Count; i++)
                    {
                        City prevCity = path[i - 1];
                        City city = path[i];
                        total += prevCity.Roads[city];
                        string tabS = city.Name.PadRight(tab) + total;
                        tabLength = Math.Max(tabLength, tabS.Length);
                        Console.WriteLine(tabS);
                    }
                    Console.WriteLine("".PadLeft(tabLength, '='));
                    Console.WriteLine("total:".PadRight(tab) + total);
                    System.IO.File.WriteAllText("output.txt", total.ToString());
                    break;
            }
            Console.ReadLine();
        }
    }
}

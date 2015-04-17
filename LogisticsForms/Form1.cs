using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LogisticsTest;

namespace LogisticsForms
{
    public partial class Form1 : Form
    {
        Random Rnd = new Random();

        Map CityMap = new Map();
        Label SelectedCity;
        Font F = new Font("Arial", 10, GraphicsUnit.Pixel);

        City Start;
        City Target;
        List<City> Path = new List<City>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CityMap = new Map();

            List<Label> cities = new List<Label>();
            List<Label> ingame = new List<Label>();

            foreach (Control c in Controls)
            {
                if (c.GetType() == typeof(Label))
                {
                    cities.Add((Label)c);
                }
            }

            for (int i = 0;i<cities.Count*2;i++)
            {
                Label c1 = cities[Rnd.Next(0, cities.Count)];
                Label c2 = cities[Rnd.Next(0, cities.Count)];
                ingame.Add(c1);
                ingame.Add(c2);
                CityMap.AddRoad(c1.Text, c2.Text, GetDistance(c1, c2));
            }
            ingame = new List<Label>(ingame.Distinct());
            SelectedCity = null;
            Start = CityMap.Cities[ingame[Rnd.Next(0, ingame.Count)].Text];
            Target = CityMap.Cities[ingame[Rnd.Next(0, ingame.Count)].Text];
            Inval();
        }

        private void label_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (Target == null)
                {
                    if (Start == null)
                        Start = CityMap.Cities[((Label)sender).Text];
                    else
                        Target = CityMap.Cities[((Label)sender).Text];
                }
                else
                {
                    Start = CityMap.Cities[((Label)sender).Text];
                    Target = null;
                }
            }
            else
            {
                if (SelectedCity == null)
                {
                    SelectedCity = (Label)sender;
                }
                else
                {
                    Label city = (Label)sender;
                    CityMap.AddRoad(SelectedCity.Text, city.Text, GetDistance(SelectedCity, city));
                    SelectedCity = null;
                }
            }
            Inval();
        }

        double GetDistance(Label city1, Label city2)
        {
            double dX = Math.Abs(city1.Left - city2.Left);
            double dY = Math.Abs(city1.Top - city2.Top);

            return Math.Round(Math.Sqrt(dX * dX + dY * dY) * (Rnd.NextDouble() + 0.5), 1);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            List<City> drowed = new List<City>();
            foreach (City city1 in CityMap.Cities.Values)
            {
                foreach (City city2 in city1.Roads.Keys)
                {
                    if (!drowed.Contains(city2))
                    {
                        Pen p = Pens.Gray;

                        for (int i = 0; i < Path.Count - 1; i++)
                        {
                            City pc1 = Path[i];
                            City pc2 = Path[i + 1];
                            if ((city1 == pc1 && city2 == pc2) || (city1 == pc2 && city2 == pc1))
                            {
                                p = new Pen(Brushes.Lime, 2f);
                                break;
                            }
                        }

                        Point loc1 = GetLabelByName(city1.Name).Location;
                        loc1.Offset(20, 7);
                        Point loc2 = GetLabelByName(city2.Name).Location;
                        loc2.Offset(20, 7);
                        Point centre = new Point((loc1.X + loc2.X) / 2, (loc1.Y + loc2.Y) / 2);
                        e.Graphics.DrawLine(p, loc1, loc2);
                        e.Graphics.DrawString(city1.Roads[city2].ToString(), F, Brushes.DarkBlue, centre);
                    }
                }
                drowed.Add(city1);
            }

        }

        private void Inval()
        {
            foreach (Control c in Controls)
            {
                if (c.GetType() == typeof(Label))
                {
                    Label l = (Label)c;
                    if ((Start != null && l.Text == Start.Name) || (Target != null && l.Text == Target.Name))
                        l.BackColor = Color.Lime;
                    else
                        l.BackColor = Color.WhiteSmoke;
                }
            }
            
            if (Start != null && Target != null)
                Path = Logistics.CalculatePath(Start, Target);
            else
                Path = new List<City>();

            pictureBox1.Invalidate();
        }

        private Label GetLabelByName(string name)
        {
            foreach (Control c in Controls)
            {
                if (c.Text == name)
                    return (Label)c;
            }
            return null;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                checkBox1.Text = "path";
            else
                checkBox1.Text = "roads";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string input = string.Format("{0} {1}", Start.Name, Target.Name);
            List<City> saved = new List<City>();
            foreach (City city1 in CityMap.Cities.Values)
            {
                foreach (City city2 in city1.Roads.Keys)
                {
                    if (!saved.Contains(city2))
                        input += string.Format("\r\n{0} {1} {2}", city1.Name, city2.Name, city1.Roads[city2]);
                }
                saved.Add(city1);
            }
            System.IO.File.WriteAllText("input.txt", input);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Map newMap = new Map();

                string[] lines = System.IO.File.ReadAllLines("input.txt");
                string[] line0 = lines[0].Split(' ');
                line0 = line0.Where(item => item != "").ToArray();

                for (int i = 1; i < lines.Length;i++ )
                {
                    string[] line = lines[i].Split(' ');
                    line = line.Where(item => item != "").ToArray();
                    if (line.Length == 3)
                        newMap.AddRoad(line[0], line[1], double.Parse(line[2]));
                }

                CityMap = newMap;
                Start = newMap.Cities[line0[0]];
                Target = newMap.Cities[line0[1]];
                Inval();
            }
            catch { }
        }
    }
}

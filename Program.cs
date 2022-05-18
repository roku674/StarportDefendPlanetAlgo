//Created by Alexander Fields http://alexanderfields.me
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StarportDefendPlanetAlgo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string planetMapPath = Directory.GetCurrentDirectory() + "/PlanetMap.json";
            string planetMapDefendedPath = Directory.GetCurrentDirectory() + "/PlanetMapDefended.json";

            int[,] mapGrid = new int[25, 25];

            /*
            mapGrid[12, 12] = 5;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    mapGrid[10 + i, 19 + j] = 1;
                }
            }

            mapGrid[10, 19] = 2;
            mapGrid[11, 19] = 2;
            mapGrid[10, 20] = 2;
            mapGrid[11, 20] = 2;

            mapGrid[13, 13] = 1;
            mapGrid[14, 14] = 1;
            mapGrid[15, 15] = 1;
            mapGrid[15, 16] = 1;
            mapGrid[15, 17] = 1;
            mapGrid[16, 17] = 1;
            mapGrid[15, 18] = 1;
            */
            Random rnd = new Random();

            //for randomized map
            for (int i = 1; i < mapGrid.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < mapGrid.GetLength(1) - 1; j++)
                {
                    int random = rnd.Next(1, 5);
                    if (random > 1)
                    {
                        mapGrid[i, j] = 1;
                    }
                }
            }

            int biodomeX = rnd.Next(1, 24);
            int biodomeY = rnd.Next(1, 24);
            mapGrid[biodomeX, biodomeY] = 2;
            mapGrid[biodomeX, biodomeY + 1] = 2;
            mapGrid[biodomeX + 1, biodomeY] = 2;
            mapGrid[biodomeX + 1, biodomeY + 1] = 2;

            mapGrid[rnd.Next(1, 25), rnd.Next(1, 25)] = 5;
            /*
            int count = 0;
            List<int[]> mapGridList = mapGrid.Cast<int>()
                    .GroupBy(x => count++ / mapGrid.GetLength(1))
                    .Select(g => g.ToArray())
                    .ToList();
            */

            string jsonBefore = JsonConvert.SerializeObject(mapGrid);
            File.WriteAllText(planetMapPath, jsonBefore);
            Console.WriteLine("Undefended Random Map Serialized!");

            string file = File.ReadAllText(planetMapPath);
            Console.WriteLine("Map Read!");

            System.Diagnostics.Debug.WriteLine("Before");

            for (int i = 0; i < mapGrid.GetLength(0); i++)
            {
                for (int j = 0; j < mapGrid.GetLength(1); j++)
                {
                    char letter = ' ';
                    if (mapGrid[i, j] == 0)
                    {
                        letter = 'W';
                    }
                    else if (mapGrid[i, j] == 1)
                    {
                    }
                    else if (mapGrid[i, j] == 2)
                    {
                        letter = 'B';
                    }
                    else if (mapGrid[i, j] == 3)
                    {
                        letter = 'C';
                    }
                    else if (mapGrid[i, j] == 4)
                    {
                        letter = 'L';
                    }
                    else if (mapGrid[i, j] == 5)
                    {
                        letter = 'E';
                    }
                    System.Diagnostics.Debug.Write(letter + "\t");
                }
                System.Diagnostics.Debug.WriteLine("");
            }

            DefensePlan defensePlan = new DefensePlan(mapGrid);
            mapGrid = defensePlan.CreateDefensePlan();

            string jsonAfter = JsonConvert.SerializeObject(mapGrid);
            File.WriteAllText(planetMapDefendedPath, jsonAfter);
            Console.WriteLine("Defended Planet Map Serialized!");

            for (int i = 0; i < mapGrid.GetLength(0); i++)
            {
                for (int j = 0; j < mapGrid.GetLength(1); j++)
                {
                    char letter = ' ';
                    if (mapGrid[i, j] == 0)
                    {
                        letter = 'W';
                    }
                    else if (mapGrid[i, j] == 1)
                    {
                    }
                    else if (mapGrid[i, j] == 2)
                    {
                        letter = 'B';
                    }
                    else if (mapGrid[i, j] == 3)
                    {
                        letter = 'C';
                    }
                    else if (mapGrid[i, j] == 4)
                    {
                        letter = 'L';
                    }
                    else if (mapGrid[i, j] == 5)
                    {
                        letter = 'E';
                    }
                    System.Diagnostics.Debug.Write(letter + "\t");
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }
    }
}
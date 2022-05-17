//Created by Alexander Fields http://alexanderfields.me
using System;
using System.IO;

namespace StarportDefendPlanetAlgo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string planetMapPath = Directory.GetCurrentDirectory() + "/PlanetMap.json";

            string jsonText = File.ReadAllText(planetMapPath);

            int[,] testGrid = new int[25, 25];
            /*
            testGrid[12, 12] = 5;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    testGrid[10 + i, 19 + j] = 1;
                }
            }

            testGrid[10, 19] = 2;
            testGrid[11, 19] = 2;
            testGrid[10, 20] = 2;
            testGrid[11, 20] = 2;

            testGrid[13, 13] = 1;
            testGrid[14, 14] = 1;
            testGrid[15, 15] = 1;
            testGrid[15, 16] = 1;
            testGrid[15, 17] = 1;
            testGrid[16, 17] = 1;
            testGrid[15, 18] = 1;
*/
            Random rnd = new Random();

            //for randomized map
            for (int i = 1; i < testGrid.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < testGrid.GetLength(1) - 1; j++)
                {
                    int random = rnd.Next(1, 5);
                    if (random > 1)
                    {
                        testGrid[i, j] = 1;
                    }
                }
            }

            int biodomeX = rnd.Next(1, 24);
            int biodomeY = rnd.Next(1, 24);
            testGrid[biodomeX, biodomeY] = 2;
            testGrid[biodomeX, biodomeY + 1] = 2;
            testGrid[biodomeX + 1, biodomeY] = 2;
            testGrid[biodomeX + 1, biodomeY + 1] = 2;

            testGrid[rnd.Next(1, 25), rnd.Next(1, 25)] = 5;

            Console.WriteLine("PlanetMap Serialized!");

            System.Diagnostics.Debug.WriteLine("Before");

            for (int i = 0; i < testGrid.GetLength(0); i++)
            {
                for (int j = 0; j < testGrid.GetLength(1); j++)
                {
                    char letter = ' ';
                    if (testGrid[i, j] == 0)
                    {
                        letter = 'W';
                    }
                    else if (testGrid[i, j] == 1)
                    {
                    }
                    else if (testGrid[i, j] == 2)
                    {
                        letter = 'B';
                    }
                    else if (testGrid[i, j] == 3)
                    {
                        letter = 'C';
                    }
                    else if (testGrid[i, j] == 4)
                    {
                        letter = 'L';
                    }
                    else if (testGrid[i, j] == 5)
                    {
                        letter = 'E';
                    }
                    System.Diagnostics.Debug.Write(letter + "\t");
                }
                System.Diagnostics.Debug.WriteLine("");
            }

            DefensePlan defensePlan = new DefensePlan(testGrid);
            testGrid = defensePlan.CreateDefensePlan();

            for (int i = 0; i < testGrid.GetLength(0); i++)
            {
                for (int j = 0; j < testGrid.GetLength(1); j++)
                {
                    char letter = ' ';
                    if (testGrid[i, j] == 0)
                    {
                        letter = 'W';
                    }
                    else if (testGrid[i, j] == 1)
                    {
                    }
                    else if (testGrid[i, j] == 2)
                    {
                        letter = 'B';
                    }
                    else if (testGrid[i, j] == 3)
                    {
                        letter = 'C';
                    }
                    else if (testGrid[i, j] == 4)
                    {
                        letter = 'L';
                    }
                    else if (testGrid[i, j] == 5)
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
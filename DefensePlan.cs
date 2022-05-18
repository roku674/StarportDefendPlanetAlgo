//Created by Alexander Fields http://alexanderfields.me
using System;
using System.Collections.Generic;

namespace StarportDefendPlanetAlgo
{
    public class DefensePlan
    {
        private static readonly int maxDefenses = 125;

        // Direction vectors
        private static int[] dCol = { 0, 1, 0, -1 };

        private static int[] dRow = { -1, 0, 1, 0 };

        /// <summary>
        /// 0-3 Biodome squares, 4 is the exit 5-8 dd
        /// </summary>
        private coordinate[] biodomeAndExit;

        private int currentDefenses;

        /// <summary>
        /// Pass in the size of the grid for the constructor
        /// <br>
        /// Biodome is 2, open is 1, walls are 0, cmines are 3, lasers are 4, 5 is warp
        /// </br>
        /// </summary>
        /// <param name="gridSize"></param>
        public DefensePlan(int[,] _planetGrid)
        {
            planetGrid = _planetGrid;
            planetGrid = _planetGrid;
        }

        /// <summary>
        /// Biodome is 2, open is 1, walls are 0, cmines are 3, lasers are 4, 5 is warp
        /// </summary>
        public static int[,] planetGrid { get; set; }

        public int[,] CreateDefensePlan()
        {
            //Find biodome
            coordinate[] map = ScanMap();

            int shortestDist = 999999;
            coordinate biodomeEdge = new coordinate();

            //get distance to exit
            for (int i = 0; i < biodomeAndExit.Length - 1; i++)
            {
                int manhattan = shortestDist;
                if (i != 4)
                {
                    manhattan = ManhattanDist((int)biodomeAndExit[i].x, (int)biodomeAndExit[i].y,
                              (int)biodomeAndExit[4].x, (int)biodomeAndExit[4].y);
                }

                if (manhattan < shortestDist)
                {
                    shortestDist = manhattan;
                    biodomeEdge = biodomeAndExit[i];
                    //System.Diagnostics.Debug.WriteLine("shortestDist: " + shortestDist);
                }
            }

            List<coordinate> coordinates = BFS(new List<coordinate>(), planetGrid, new bool[planetGrid.GetLength(0), planetGrid.GetLength(1)], (int)biodomeEdge.x, (int)biodomeEdge.y);
            bool passedExit = false;
            for (int i = 0; i < coordinates.Count; i++)
            {
                if (planetGrid[coordinates[i].x, coordinates[i].y] == 5)
                {
                    passedExit = true;
                    System.Diagnostics.Debug.WriteLine("Passed Exit");
                }
                //System.Diagnostics.Debug.WriteLine("(" + coordinates[i].x + " , " + coordinates[i].y + "): " + planetGrid[coordinates[i].x, coordinates[i].y] + " ");
                if (planetGrid[coordinates[i].x, coordinates[i].y] == 1 && currentDefenses < maxDefenses)
                {
                    if (!passedExit && (
                        (planetGrid[coordinates[i].x - 1, coordinates[i].y] == 0 && planetGrid[coordinates[i].x + 1, coordinates[i].y] == 0 &&
                        planetGrid[coordinates[i].x, coordinates[i].y - 1] != 0 && planetGrid[coordinates[i].x, coordinates[i].y + 1] != 0) ||
                        (planetGrid[coordinates[i].x - 1, coordinates[i].y] != 0 && planetGrid[coordinates[i].x + 1, coordinates[i].y] != 0 &&
                        planetGrid[coordinates[i].x, coordinates[i].y - 1] == 0 && planetGrid[coordinates[i].x, coordinates[i].y + 1] == 0))
                    )
                    {
                        planetGrid[coordinates[i].x, coordinates[i].y] = 3;
                        currentDefenses++;
                    }
                    else
                    {
                        planetGrid[coordinates[i].x, coordinates[i].y] = 4;
                        currentDefenses++;
                    }

                    uint walls = 0;
                    //check all wals nearby
                    if (planetGrid[coordinates[i].x - 1, coordinates[i].y] == 0) { walls++; }
                    if (planetGrid[coordinates[i].x, coordinates[i].y - 1] == 0) { walls++; }
                    if (planetGrid[coordinates[i].x - 1, coordinates[i].y - 1] == 0) { walls++; }
                    if (planetGrid[coordinates[i].x + 1, coordinates[i].y] == 0) { walls++; }
                    if (planetGrid[coordinates[i].x, coordinates[i].y + 1] == 0) { walls++; }
                    if (planetGrid[coordinates[i].x + 1, coordinates[i].y + 1] == 0) { walls++; }
                    if (planetGrid[coordinates[i].x + 1, coordinates[i].y - 1] == 0) { walls++; }
                    if (planetGrid[coordinates[i].x - 1, coordinates[i].y + 1] == 0) { walls++; }

                    if (walls >= 6 && !passedExit)
                    {
                        planetGrid[coordinates[i].x, coordinates[i].y] = 3;
                        currentDefenses++;
                    }

                    for (int j = 0; j < 5; j++)
                    {
                        //if position in list has a biodome cmine near
                        if (i + j < coordinates.Count && planetGrid[coordinates[i + j].x, coordinates[i + j].y] == 2)
                        {
                            planetGrid[coordinates[i].x, coordinates[i].y] = 3;
                            currentDefenses++;
                        }
                        if (i - j > 0 && planetGrid[coordinates[i - j].x, coordinates[i - j].y] == 2)
                        {
                            planetGrid[coordinates[i].x, coordinates[i].y] = 3;
                            currentDefenses++;
                        }
                    }
                }
                else if (planetGrid[coordinates[i].x, coordinates[i].y] == 5 && currentDefenses == maxDefenses)
                {
                    break;
                }
                else if (currentDefenses == maxDefenses)
                {
                    break;
                }
            }

            System.Diagnostics.Debug.WriteLine("currentDefenses: " + currentDefenses);

            return planetGrid;
        }

        /// <summary>
        /// Breadth First Traversal ( BFS ) on a 2D array <br>Function to perform the BFS traversal.</br>
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="grid"></param>
        /// <param name="vis"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>coordinate list</returns>
        private List<coordinate> BFS(List<coordinate> coordinates, int[,] grid, bool[,] vis,
                        int row, int col)
        {
            // Stores indices of the matrix cells
            Queue<pair> q = new Queue<pair>();

            // Mark the starting cell as visited
            // and push it into the queue
            q.Enqueue(new pair(row, col));
            vis[row, col] = true;

            // Iterate while the queue
            // is not empty
            while (q.Count != 0)
            {
                pair cell = q.Peek();
                int x = cell.first;
                int y = cell.second;

                //System.Diagnostics.Debug.WriteLine("(" + x + " , " + y + "): " + grid[x, y] + " ");
                coordinate coord = new coordinate(x, y);
                if (!coordinates.Contains(coord))
                {
                    coordinates.Add(coord);
                }

                q.Dequeue();

                // Go to the adjacent cells
                for (int i = 0; i < 4; i++)
                {
                    int adjx = x + dRow[i];
                    int adjy = y + dCol[i];
                    if (isValid(vis, adjx, adjy))
                    {
                        q.Enqueue(new pair(adjx, adjy));
                        vis[adjx, adjy] = true;
                    }
                }
            }

            return coordinates;
        }

        /// <summary>
        /// Function to check if a cell
        /// is be visited or not
        /// </summary>
        /// <param name="vis"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private static bool isValid(bool[,] vis,
                            int row, int col)
        {
            // If cell lies out of bounds
            if (row < 0 || col < 0 ||
                row >= planetGrid.GetLength(0) || col >= planetGrid.GetLength(1))
                return false;

            // If cell is already visited
            if (vis[row, col])
                return false;

            // Otherwise
            return true;
        }

        private int ManhattanDist(int x1, int y1,
                                   int x2, int y2)
        {
            int dist = Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
            return (int)dist;
        }

        /// <summary>
        /// First 4 coordinates are the
        /// </summary>
        /// <returns></returns>
        private coordinate[] ScanMap()
        {
            biodomeAndExit = new coordinate[9];
            int biodomeCount = 0;

            for (int i = 0; i < planetGrid.GetLength(0); i++)
            {
                for (int j = 0; j < planetGrid.GetLength(1); j++)
                {
                    if (planetGrid[i, j] == 2)
                    {
                        biodomeAndExit[biodomeCount].x = i;
                        biodomeAndExit[biodomeCount].y = j;
                        biodomeCount++;
                    }
                    else if (planetGrid[i, j] == 3 || planetGrid[i, j] == 4)
                    {
                        currentDefenses++;
                    }
                    else if (planetGrid[i, j] == 5)
                    {
                        biodomeAndExit[4].x = i;
                        biodomeAndExit[4].y = j;
                    }
                }
            }

            if (biodomeCount == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    biodomeAndExit[5 + i] = biodomeAndExit[0 + i];
                }
            }

            return biodomeAndExit;
        }

        /// <summary>
        /// Biodome is 2, open is 1, walls are 0, cmines are 3, lasers are 4, 5 is warp
        /// </summary>
        private struct coordinate
        {
            public coordinate(int _x, int _y)
            {
                x = _x;
                y = _y;
            }

            public int x { get; set; }
            public int y { get; set; }
        }

        private struct pair
        {
            public int first, second;

            public pair(int first, int second)
            {
                this.first = first;
                this.second = second;
            }
        }
    }
}
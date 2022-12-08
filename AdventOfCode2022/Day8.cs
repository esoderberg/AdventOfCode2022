using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day8 : AoCDay
    {


        public struct Tree
        {
            public int Height;
            public int TallestLeft = -1;
            public int TallestTop = -1;
            public int TallestRight = -1;
            public int TallestBottom = -1;
            public bool IsVisible => Height > TallestLeft || Height > TallestTop || Height > TallestBottom || Height > TallestRight;
            public Tree(int height) { Height = height; }
        }


        public static Tree[,] ParseGrid(List<string> input)
        {
            int width = input[0].Length;
            int height = input.Count;
            Tree[,] grid = new Tree[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[x, y] = new Tree(input[y][x] - '0');
                }
            }
            return grid;
        }

        /// <summary>
        /// Update each tree with the tallest trees on its sides.
        /// </summary>
        public static void UpdateWithTallestTrees(Tree[,] treeGrid)
        {
            int width = treeGrid.GetLength(0);
            int height = treeGrid.GetLength(1);
            // Start on the 2nd leftmost column going right, storing the tallest leftside tree
            for (int y = 0; y < height; y++)
            {
                for (int x = 1; x < width; x++)
                {
                    treeGrid[x, y].TallestLeft = Math.Max(treeGrid[x - 1, y].TallestLeft, treeGrid[x-1, y].Height);
                }
            }

            // Start on the 2nd rightmost column going left, storing the tallest rightside tree
            for (int y = 0; y < height; y++)
            {
                for (int x = width-2; x >= 0; x--)
                {
                    treeGrid[x, y].TallestRight = Math.Max(treeGrid[x + 1, y].TallestRight, treeGrid[x + 1, y].Height);
                }
            }

            // Start on the 2nd top row going down, storing the tallest topside tree.
            for (int x = 0; x < width; x++)
            {
                for (int y = 1; y < height; y++)
                {
                    treeGrid[x, y].TallestTop = Math.Max(treeGrid[x, y - 1].TallestTop, treeGrid[x, y - 1].Height);
                }
            }

            // Start on the 2nd bottom row going down, storing the tallest topside tree.
            for (int x = 0; x < width; x++)
            {
                for (int y = height - 2; y >= 0 ; y--)
                {
                    treeGrid[x, y].TallestBottom = Math.Max(treeGrid[x, y + 1].TallestBottom, treeGrid[x, y + 1].Height);
                }
            }
        }
        /// <summary>
        /// Count the number of trees that are visible.
        /// Grid must be preprocessed by UpdateWithTallestTrees.
        public static int CountVisibleTrees(Tree[,] treeGrid)
        {
            int width = treeGrid.GetLength(0);
            int height = treeGrid.GetLength(1);
            int visible = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    visible += treeGrid[x, y].IsVisible ? 1 : 0;
                }
            }
            return visible;
        }
        public static void PrintTrees(Tree[,] treeGrid)
        {
            int width = treeGrid.GetLength(0);
            int height = treeGrid.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    System.Diagnostics.Debug.Write(treeGrid[x, y].IsVisible ? "1": 0);
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }

        public static string ExecutePart1(List<string> input)
        {
            var treeGrid = ParseGrid(input);
            UpdateWithTallestTrees(treeGrid);
            //PrintTrees(treeGrid);
            return CountVisibleTrees(treeGrid).ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            throw new NotImplementedException();
        }
    }
}

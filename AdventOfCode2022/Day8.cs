using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day8 : AoCDay
    {


        public class Tree
        {
            public int Height;
            public int x;
            public int y;
            public Tree? TallestLeft = null;
            public Tree? TallestTop = null;
            public Tree? TallestRight = null;
            public Tree? TallestBottom = null;
            public int ViewDistLeft = 0;
            public int ViewDistTop = 0;
            public int ViewDistRight = 0;
            public int ViewDistBottom = 0;
            public bool IsVisible => this > TallestLeft || this > TallestTop || this > TallestBottom || this > TallestRight;
            public int ScenicScore => ViewDistLeft * ViewDistTop * ViewDistBottom * ViewDistRight;
            public Tree(int height, int x, int y)
            {
                Height = height;
                this.x = x;
                this.y = y;
            }
            // A tree is always taller than no tree.
            public static bool operator >(Tree a, Tree? b) => b == null? true : a.Height > b?.Height;
            
            // A tree can't be shorter than no tree.
            public static bool operator <(Tree a, Tree? b) => b == null? false : a.Height < b?.Height;

            public int Distance(Tree? b)
            {
                if (b == null) return 0;
                else return Math.Abs(x - b.x) + Math.Abs(y - b.y);
            }
            public static Tree? Tallest(Tree? a, Tree? b)
            {
                if (a == null) return b;
                else if (b == null) return a;
                else return a > b ? a : b;
            }
            public override string ToString()
            {
                return $"Tree[{x},{y}]({Height})";
            }
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
                    grid[x, y] = new Tree(input[y][x] - '0', x, y);
                }
            }
            return grid;
        }

        public static Tree? GetNearestBlockingTree(Tree tree, Tree[] trees)
        {
            int nearestDistance = int.MaxValue;
            Tree? blockingTree = null;
            for (int i = tree.Height; i < trees.Length; i++)
            {
                if (trees[i] != null)
                {
                    if (blockingTree == null)
                    {
                        blockingTree = trees[i];
                        nearestDistance = tree.Distance(trees[i]);
                    }
                    else
                    {
                        int distance = tree.Distance(trees[i]);
                        blockingTree = distance < nearestDistance ? trees[i] : blockingTree;
                        nearestDistance = distance;
                    }
                }
            }
            return blockingTree;
        }
        /// <summary>
        /// Update each tree with the tallest trees on its sides.
        /// </summary>
        public static void UpdateWithTallestTreesAndSightDistances(Tree[,] treeGrid)
        {
            int width = treeGrid.GetLength(0);
            int height = treeGrid.GetLength(1);
            // Start on the 2nd leftmost column going right, storing the tallest leftside tree
            
            for (int y = 0; y < height; y++)
            {
                Tree[] trees = new Tree[10];
                trees[treeGrid[0, y].Height] = treeGrid[0, y];
                for (int x = 1; x < width; x++)
                {
                    Tree tree = treeGrid[x, y];
                    Tree leftTree = treeGrid[x - 1, y];
                    tree.TallestLeft = Tree.Tallest(leftTree.TallestLeft, leftTree);
                    tree.ViewDistLeft = tree.Distance(GetNearestBlockingTree(tree, trees));
                    tree.ViewDistLeft = tree.ViewDistLeft == 0 ? tree.x : tree.ViewDistLeft;
                    trees[tree.Height] = tree;
                }
            }

            // Start on the 2nd rightmost column going left, storing the tallest rightside tree
            for (int y = 0; y < height; y++)
            {
                Tree[] trees = new Tree[10];
                trees[treeGrid[width - 1, y].Height] = treeGrid[width - 1, y];
                for (int x = width-2; x >= 0; x--)
                {
                    Tree tree = treeGrid[x, y];
                    Tree rightTree = treeGrid[x + 1, y];
                    tree.TallestRight = Tree.Tallest(rightTree.TallestRight, rightTree);
                    tree.ViewDistRight = tree.Distance(GetNearestBlockingTree(tree, trees));
                    tree.ViewDistRight = tree.ViewDistRight == 0 ? width - 1 - tree.x: tree.ViewDistRight;
                    trees[tree.Height] = tree;
                }
            }
            // Start on the 2nd top row going down, storing the tallest topside tree.
            for (int x = 0; x < width; x++)
            {
                Tree[] trees = new Tree[10];

                trees[treeGrid[x, 0].Height] = treeGrid[x, 0];
                for (int y = 1; y < height; y++)
                {
                    Tree tree = treeGrid[x, y];
                    Tree topTree = treeGrid[x, y - 1];
                    tree.TallestTop = Tree.Tallest(topTree.TallestTop, topTree);
                    tree.ViewDistTop = tree.Distance(GetNearestBlockingTree(tree, trees));
                    tree.ViewDistTop = tree.ViewDistTop == 0 ? tree.y : tree.ViewDistTop;
                    trees[tree.Height] = tree;
                }
            }
            // Start on the 2nd bottom row going down, storing the tallest topside tree.
            for (int x = 0; x < width; x++)
            {
                Tree[] trees = new Tree[10];
                trees[treeGrid[x, height-1].Height] = treeGrid[x, height-1];
                for (int y = height - 2; y >= 0 ; y--)
                {
                    Tree tree = treeGrid[x, y];
                    Tree bottomTree = treeGrid[x, y + 1];
                    tree.TallestBottom = Tree.Tallest(bottomTree.TallestBottom, bottomTree);
                    tree.ViewDistBottom = tree.Distance(GetNearestBlockingTree(tree, trees));
                    tree.ViewDistBottom = tree.ViewDistBottom == 0 ? height - 1 - tree.y : tree.ViewDistBottom;
                    trees[tree.Height] = tree;
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

        public static int FindLargestScenicScore(Tree[,] treeGrid)
        {
            int width = treeGrid.GetLength(0);
            int height = treeGrid.GetLength(1);
            int scenicMax = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    scenicMax = Math.Max(scenicMax, treeGrid[x, y].ScenicScore);
                }
            }
            return scenicMax;
        }
        

        public static void PrintTreeVisible(Tree[,] treeGrid)
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
        public static void PrintTreeScenic(Tree[,] treeGrid)
        {
            int width = treeGrid.GetLength(0);
            int height = treeGrid.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    System.Diagnostics.Debug.Write(treeGrid[x, y].ScenicScore.ToString());
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }

        public static string ExecutePart1(List<string> input)
        {
            var treeGrid = ParseGrid(input);
            UpdateWithTallestTreesAndSightDistances(treeGrid);
            //PrintTrees(treeGrid);
            return CountVisibleTrees(treeGrid).ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            var treeGrid = ParseGrid(input);
            UpdateWithTallestTreesAndSightDistances(treeGrid);
            //PrintTreeScenic(treeGrid);
            return FindLargestScenicScore(treeGrid).ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day7 : AoCDay
    {


        //record FileSystemNode(string Name);
        //record Directory(string Name) : FileSystemNode(Name);
        //record File(int Size, string Name) : FileSystemNode(Name);
        abstract class FileSystemNode
        {
            public string Name { get; set; }
            public Directory? Parent { get; set; }
            public abstract int Size { get; }
            public FileSystemNode(string Name)
            {
                this.Name = Name;
            }

        }

        class Directory : FileSystemNode
        {
            private Dictionary<string, FileSystemNode> children = new();
            public Directory(string Name) : base(Name)
            { }
            public void AddChild(FileSystemNode child)
            { 
                child.Parent = this;
                children.Add(child.Name, child);
            }
            /// <summary>
            /// Get directory by name, throws exception if no such directory exists.
            /// </summary>
            /// <param name="dir"></param>
            /// <returns></returns>
            public Directory GetDir(string dir){ return (Directory) children[dir]; }

            public override int Size { get => children.Values.Sum(c => c.Size); }

            public List<Directory> GetDirectoriesWhere(Func<Directory, bool> predicate)
            {
                var childDirs = children.Values.Where(c => c is Directory).Cast<Directory>();
                return childDirs.Where(predicate).Union(childDirs.SelectMany(c => c.GetDirectoriesWhere(predicate))).ToList();
            }

        }
        class File : FileSystemNode
        {
            private int size;
            public File(string Name, int size) : base(Name)
            {
                this.size = size;
            }

            public override int Size => size;
        }

        class FileSystem
        {
            public readonly Directory Root = new Directory("/");
            private Directory CurrentDirectory;
            public FileSystem()
            {
                CurrentDirectory = Root;
            }
            public void ExecuteCd(string dir)
            {
                if (CurrentDirectory == null) { CurrentDirectory = new Directory(dir); }
                else if (dir == "..") CurrentDirectory = CurrentDirectory?.Parent;
                else if (dir == "/")
                {
                    CurrentDirectory = Root;
                }
                else { CurrentDirectory = CurrentDirectory.GetDir(dir); }
            }

            public void ParseLsResults(List<string> results)
            {
                foreach (var item in results)
                {
                    CurrentDirectory?.AddChild(ParseLsResult(item));
                }
            }
            private FileSystemNode ParseLsResult(string item)
            {
                switch (item)
                {
                    case var s when s.StartsWith("dir"): return new Directory(s.Split(" ")[1]);
                    default:
                        var fileparts = item.Split(" ");
                        return new File(fileparts[1], int.Parse(fileparts[0]));
                }
            }
        }

        static FileSystem ParseCommands(List<string> input)
        {
            FileSystem fs = new FileSystem();
            for (int i = 0; i < input.Count; i++)
            {
                var line = input[i];
                switch (line)
                {
                    case var s when s.StartsWith("$ cd"):
                        fs.ExecuteCd(s.Split(" ")[2]);
                        break;
                    case var s when s.StartsWith("$ ls"):
                        int lsCount = 0;
                        for (; i + 1 + lsCount < input.Count; lsCount++)
                            if (input[i + 1 + lsCount].StartsWith("$")) break;
                        fs.ParseLsResults(input.GetRange(i + 1, lsCount));
                        i += lsCount;
                        break;
                    case var s when s.StartsWith("dir"):
                        break;
                    default:
                        throw new InvalidDataException("Non-matching command");
                }
            }
            fs.ExecuteCd("/");
            return fs;
        }


        public static string ExecutePart1(List<string> input)
        {
            var fs = ParseCommands(input);
            return fs.Root.GetDirectoriesWhere((d => d.Size <= 100000)).Sum(d => d.Size).ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            int TotalDiskSpace = 70000000;
            int NeededDiskSpace = 30000000;
            var fs = ParseCommands(input);
            int UsedDiskSpace = fs.Root.Size;
            int FreeSpace = TotalDiskSpace - UsedDiskSpace;
            int MinimumDeleteRequired = NeededDiskSpace - FreeSpace;
            return fs.Root.GetDirectoriesWhere((d => d.Size >= MinimumDeleteRequired)).MinBy(d => d.Size).Size.ToString();
        }
    }
}

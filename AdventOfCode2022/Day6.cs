using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    /// <summary>
    /// Ring buffer that only keeps a certain number of elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RingBuffer<T>
    {
        private readonly T[] array;
        private readonly bool[] itemPresent;
        private int oldestIndex = 0;
        private int addIndex = 0;
        public RingBuffer(int size) 
        { 
            array = new T[size]; 
            itemPresent = new bool[size];
        }

        public void Add(T item)
        {
            array[addIndex] = item;
            addIndex++;
            if (itemPresent[addIndex-1]) oldestIndex = addIndex;
            itemPresent[addIndex-1] = true;
            
            if (addIndex >= array.Length) addIndex = 0;
        }
        public void Add(params T[] items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }
        
        public bool Contains(T item)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(array[i], item)) return true;
            }
            return false;
        }
        public int IndexOf(T item)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(array[(oldestIndex + i) % array.Length], item)) return i;
            }
            return -1;
        }

        public bool HasDuplicateItem()
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i+1; j < array.Length; j++)
                {
                    if (EqualityComparer<T>.Default.Equals(array[i], array[j])) return true;
                }
            }
            return false;
        }
    }


    public class Day6 : AoCDay
    {
        public static string ExecutePart1(List<string> input)
        {
            string line = input.First();
            RingBuffer<char> buffer = new RingBuffer<char>(4);
            buffer.Add(line.Take(3).ToArray());
            int characters = 3;
            for (int i = 3; i < line.Length; i++)
            {
                buffer.Add(line[i]);
                characters++;
                if (!buffer.HasDuplicateItem()) break;
            }
            return characters.ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            throw new NotImplementedException();
        }
    }
}

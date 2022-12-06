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
            return SolveWithFixedSizeArray(input, 4);
        }




        public static string ExecutePart2(List<string> input)
        {
            return SolveWithFixedSizeArray(input, 14);
        }

        // Absolutely fastest, ~2s for 800mb input
        private static string SolveWithFixedSizeArray(List<string> input, int distinct_characters)
        {
            string line = input.First();
            
            int[] letter_counts = new int[26];
            int currently_distinct_chars = 0;
            // Initial handling
            for (int i = 0; i < distinct_characters; i++)
            {
                int letter_index = line[i] - 'a';
                switch(letter_counts[letter_index])
                {
                    case 0: currently_distinct_chars += 1; break;
                }
                letter_counts[letter_index]++;
            }

            if (currently_distinct_chars == distinct_characters) return distinct_characters.ToString();

            for (int i = distinct_characters; i < line.Length; i++)
            {
                int first_letter_index = line[i - distinct_characters] - 'a';
                int letter_index = line[i] - 'a';
                letter_counts[first_letter_index]--;
                if (letter_counts[first_letter_index] == 0) currently_distinct_chars--;
                if (letter_counts[letter_index] == 0) currently_distinct_chars++;
                letter_counts[letter_index]++;
                if (currently_distinct_chars == distinct_characters) return (i+1).ToString();

            }
            throw new ArgumentException($"Can't find substring with {distinct_characters} distinct letters");
        }
        // 2nd fastest
        private static string SolveWithDictionaryAndSet(List<string> input, int distinct_characters)
        {
            string line = input.First();
            int characters = distinct_characters;
            // Special handling for first segment, makes the forloop cleaner.
            var first_segment = line.Take(distinct_characters).ToList();
            var distinct_set = new HashSet<char>(first_segment);
            var letter_count = new Dictionary<char, int>();
            first_segment.ForEach((c) => letter_count[c] = letter_count.GetValueOrDefault(c) + 1);
            if (distinct_set.Count == distinct_characters) return characters.ToString();

            for (int i = distinct_characters; i < line.Length; i++)
            {
                char first_letter = line[i - distinct_characters];
                letter_count[first_letter]--;
                if (letter_count[first_letter] == 0) distinct_set.Remove(first_letter);
                distinct_set.Add(line[i]);
                letter_count[line[i]] = letter_count.GetValueOrDefault(line[i]) + 1;
                if (distinct_set.Count == distinct_characters) { characters = i + 1; /* since i is an index */ break; }
            }

            return characters.ToString();
        }
        // Slowest
        private static string SoilveWithSet(List<string> input, int distinct_characters)
        {
            string line = input.First();
            int characters = 0;
            var distinct_set = new HashSet<char>();
            for (int i = 0; i < line.Length; i++)
            {

                for (int j = 0; j < distinct_characters; j++)
                {
                    distinct_set.Add(line[i + j]);
                }
                if (distinct_set.Count == distinct_characters) { characters = distinct_characters + i; break; }
                distinct_set.Clear();
            }
            return characters.ToString();
        }

        // 3rd fastest
        private static string SolveWithRingBuffer(List<string> input, int distinct_characters)
        {
            string line = input.First();
            

            RingBuffer<char> buffer = new RingBuffer<char>(distinct_characters);
            buffer.Add(line.Take(distinct_characters - 1).ToArray());
            int characters = distinct_characters - 1;
            for (int i = characters; i < line.Length; i++)
            {
                buffer.Add(line[i]);
                characters++;
                if (!buffer.HasDuplicateItem()) break;
            }
            return characters.ToString();
        }
    }
}

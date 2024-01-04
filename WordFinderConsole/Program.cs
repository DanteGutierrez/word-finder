using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WordFinderConsole;

class Program
{
    static void Main(string[] args)
    {
        bool active = true;

        Console.WriteLine("Welcome to the word finder\n-- This is a tool powered by the Oxford English Dictionary --\n");

        while (active) {
            {
                Console.WriteLine("Enter characters below: ");

                string? input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input)) active = false;

                else
                {
                    Stopwatch stopwatch = new();

                    stopwatch.Start();

                    Dictionary<int, List<string>> result = Finder.FindAllWords(input);

                    stopwatch.Stop();

                    Console.WriteLine("Result found in: {0}s", stopwatch.ElapsedMilliseconds / 1000.0);

                    StringBuilder builder = new();

                    for (int i = input.Length; i >= 1; i--)
                    {
                        if (!result.ContainsKey(i)) continue;

                        builder.Append('\n');

                        builder.Append(i);

                        builder.Append(" Character length words:\n\n");

                        for (int j = 0; j < result[i].Count; j++)
                        {
                            if (j > 0) builder.Append(", ");

                            builder.Append(result[i][j]);
                        }

                        builder.Append("\n\n");
                    }

                    Console.WriteLine(builder.ToString());
                }
            }
        }
    }
}


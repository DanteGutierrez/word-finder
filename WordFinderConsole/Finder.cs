using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordFinderConsole;

public class Finder
{
	private static readonly Dictionary<string, string[]> Cache = new(); // Will hold all combinations for the duration of the program

	private static Dictionary<int, List<string>> WordDictionary = new();

	private static readonly string WordDictionaryFilePath = "../../../Oxford English Dictionary.txt";

	/// <summary>
	/// Will return all word combinations in a dictionary indexed by word length
	/// </summary>
	/// <param name="characters">The available characters to use</param>
	/// <returns>Dictionary of words found indexed by word length</returns>
	public static Dictionary<int, List<string>> FindAllWords(string characters)
	{
		if(characters.Length == 0) return new();

		LoadDictionary();

        characters = string.Concat(characters.OrderBy(c => c));

        List<string> foundWords = FindWordsWith(characters);

		Dictionary<int, List<string>> result = SortWordsIntoDictionaryByLength(foundWords);

		return result;
	}

	/// <summary>
	/// A recursive function that will find all words with the given combination
	/// </summary>
	/// <param name="characters">The available characters to use</param>
	/// <returns>A list with all words that were found</returns>
	private static List<string> FindWordsWith(string characters)
	{
		if (characters is null) return new();

		if (Cache.ContainsKey(characters)) return Cache[characters].ToList();

		HashSet<string> allWords = new();

		if(WordDictionary.ContainsKey(characters.Length))
		{
			foreach(string word in WordDictionary[characters.Length])
			{
				string sortedWord = string.Concat(word.OrderBy(c => c));

				bool match = true;

				for(int i = 0; i < characters.Length; i++)
				{
					if (sortedWord[i] != characters[i])
					{
						match = false;
						break;
					}
				}

                if (match) allWords.Add(word);
			}

		}

		// Start calling recursively

		for(int i = 0; i < characters.Length; i++)
		{
			StringBuilder builder = new();

			for(int j = 0; j < characters.Length; j++)
			{
				if(i != j) builder.Append(characters[j]);
			}

			string newCombination = builder.ToString();

			List<string> test = FindWordsWith(newCombination);

			allWords.UnionWith(test);

			builder.Clear();
		}

        Cache.Add(characters, allWords.ToArray());

        return allWords.ToList();
	}

	/// <summary>
	/// Loads the word list into memory
	/// </summary>
	private static void LoadDictionary()
	{
		if (WordDictionary.Count > 0) return;

		List<string> words = new();

		using(StreamReader sr = File.OpenText(WordDictionaryFilePath))
		{
			string? line;
			while((line = sr.ReadLine()?.Trim()) is not null)
			{
				if (string.IsNullOrEmpty(line)) continue;

				string refinedWord = line.Split(" ")[0].ToLower();

				if (refinedWord.StartsWith("-") || refinedWord.EndsWith("-") || refinedWord.Length < 3) continue;

				words.Add(refinedWord);
			}
		}

		Dictionary<int, List<string>> result = SortWordsIntoDictionaryByLength(words);

		WordDictionary = result;
	}

	/// <summary>
	/// Sorts words into a dictionary that is indexed by length
	/// </summary>
	/// <param name="words">A list of words to sort through</param>
	/// <returns>A dictionary indexed by length of strings</returns>
	private static Dictionary<int, List<string>> SortWordsIntoDictionaryByLength(IList<string> words)
	{
		Dictionary<int, List<string>> result = new();

        foreach (string word in words)
        {
            if (!result.ContainsKey(word.Length))
            {
                result.Add(word.Length, new List<string>() { word });
            }
            else result[word.Length].Add(word);
        }

		return result;
    }
}

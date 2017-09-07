using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Multiple words parser.
    /// </summary>
    public class MultipleWordsParser : IMultipleWordsParser
    {
        private const short MinWordLength = 2;
        private readonly DictionaryStorage _dictionaryStorage;
        private readonly DictionaryTree _dictionaryTree;


        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="dictionaryStorage">Init parser with a existing dictionary storage.</param>
        public MultipleWordsParser(DictionaryStorage dictionaryStorage)
        {
            _dictionaryStorage = dictionaryStorage;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="dictionaryTree">Construct parser with a existing dictionary storage.</param>
        public MultipleWordsParser(DictionaryTree dictionaryTree)
        {
            _dictionaryTree = dictionaryTree;
        }

        /// <summary>
        /// Finded words result.
        /// </summary>
        public Dictionary<string, List<WordsResult>> Result { get; } = new Dictionary<string, List<WordsResult>>();


        /// <summary>
        /// Parsing words.
        /// </summary>
        /// <param name="word">Input word.</param>
        /// <param name="masterWord">Main word, consisted of other subwords.</param>
        public void Parse(string word, string masterWord)
        {
            
            if (_dictionaryStorage != null)
            {
                ParseFromDictionaryStorage(word, masterWord);
            }
            else
            
            {
                ParseFromTree(word, masterWord);
            }
        }

        /// <summary>
        /// Parsing words, contained in the dictionary storage.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="masterWord"></param>
        private void ParseFromDictionaryStorage(string word, string masterWord)
        {
            for (var i = word.Length - 1; i >= 0; i--)
            {
                var subWord = word.Substring(0, i + 1);
                if (masterWord.Equals(subWord)) continue;
                
                if (_dictionaryStorage.Exists(subWord))
                {
                    if (!Result.ContainsKey(masterWord))
                    {
                        Result.Add(masterWord, new List<WordsResult>());
                        Result[masterWord].Add(new WordsResult());
                    }
                    Result[masterWord].Last().Words.Add(subWord);

                    if (subWord.Length < MinWordLength) break;
                    var newSubWord = word.Substring(subWord.Length);
                    Parse(newSubWord, masterWord);
                }
                

                if (i == 0 && Result.ContainsKey(masterWord))
                {
                    var lastItem = Result[masterWord].Last();
                    if (string.Join("", lastItem.Words) == masterWord)
                    {
                        Result[masterWord].Add(new WordsResult());
                    }
                    else
                    {
                        var count = Result[masterWord].Count;
                        Result[masterWord][count - 1] = new WordsResult();
                    }
                }
            }
        }

        /// <summary>
        /// Parsing words, contained in the dictionary tree.
        /// </summary>
        /// <param name="word">Input word.</param>
        /// <param name="masterWord">Main word, consisted of other subwords.</param>
        private void ParseFromTree(string word, string masterWord)
        {
            if (Result.ContainsKey(masterWord) && masterWord.EndsWith(word) &&
                masterWord.Equals(string.Join("", Result[masterWord].Last().Words)))
            {
                Result[masterWord].Add(new WordsResult());
            }
            var node = _dictionaryTree.Node;
            var subWord = string.Empty;
            foreach (var character in word)
            {
                if (_dictionaryTree.Node.ContainsKey(character))
                {
                    subWord += character;
                    if (subWord.Equals(masterWord))
                    {
                        continue;
                    }
                    if (!node.ContainsKey(character))
                    {
                        break;
                    }
                    if (node[character].Words.Contains(subWord))
                    {
                        if (!Result.ContainsKey(masterWord))
                        {
                            Result.Add(masterWord, new List<WordsResult>());
                            Result[masterWord].Add(new WordsResult());
                        }

                        Result[masterWord].Last().Words.Add(subWord);
                        ParseFromTree(word.Substring(subWord.Length), masterWord);
                        if (!masterWord.Equals(string.Join("", Result[masterWord].Last().Words)))
                        {
                            Result[masterWord].Last().Words = new Collection<string>();
                        }
                    }
                    node = node[character].Node;
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Save the result into the file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        public void SaveParseResult(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            foreach (var key in Result.Keys)
            {
                var line = key + " : ";
                var wordResult = Result[key];
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < wordResult.Count; i++)
                {
                    var subWords = wordResult[i];
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (var word in subWords.Words)
                    {
                        line += word + ", ";
                    }
                    line = line.TrimEnd().TrimEnd(',');
                    if (subWords.Words.Count > 0)
                    {
                        line += " | ";
                    }
                }
                line = line.TrimEnd().TrimEnd('|');
                line += Environment.NewLine;
                File.AppendAllText(path, line);
            }
        }
    }
}
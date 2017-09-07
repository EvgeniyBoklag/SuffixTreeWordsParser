using System;
using System.IO;
using System.Linq;
using Core;

namespace WordsParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var storage = new DictionaryStorage();
            storage.LoadDictionary("dict");

            Console.WriteLine("Min words length = " + storage.MinWordLength);
            Console.WriteLine("SmallestWord = " + storage.SmallestWord);

            var fileLines = File.ReadLines("de-test-words.tsv");
            var words = fileLines.Skip(1).Select(l => l.Split('\t').ToArray());

            var parser = new MultipleWordsParser(storage);
            var dt = DateTime.Now.Ticks;

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var word in words)
            {
                parser.Parse(word[1].ToLower(), word[1].ToLower());
            }

            // ReSharper disable once PossibleMultipleEnumeration
            var res = (float) ((DateTime.Now.Ticks - dt)/words.Count())/10000;

            Console.WriteLine("Result count = " + parser.Result.Count);
            Console.WriteLine("Spent time = " + res + "ms.");
            parser.SaveParseResult("WordsParserResult.txt");
            Console.WriteLine("press any key and enter to exit..");
            Console.ReadLine();
        }
    }
}
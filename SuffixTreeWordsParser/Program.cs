using System;
using System.IO;
using System.Linq;
using Core;

namespace SuffixTreeWordsParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Load dictionary...");
            var builder = new DictionaryTreeBuilder("dict");

            var parser = new MultipleWordsParser(builder.DictionaryTree);
            var fileLines = File.ReadLines("de-test-words.tsv");
            var words = fileLines.Skip(1).Select(l => l.Split('\t').ToArray());

            var dt = DateTime.Now.Ticks;

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var word in words)
            {
                parser.Parse(word[1].ToLower(), word[1].ToLower());
            }

            // ReSharper disable once PossibleMultipleEnumeration
            // ReSharper disable once PossibleLossOfFraction
            var res = (float) ((DateTime.Now.Ticks - dt)/words.Count())/10000;

            Console.WriteLine("Result count = " + parser.Result.Count);
            Console.WriteLine("Spent time = " + res + "ms.");
            parser.SaveParseResult("suffTreeResult.txt");
            Console.WriteLine("press any key and enter to exit..");
            Console.ReadLine();
        }
    }
}
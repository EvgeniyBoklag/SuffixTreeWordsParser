using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Bulder of the dictionary tree from dictionary text file.
    /// </summary>
    public class DictionaryTreeBuilder
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="dictionaryFilePath">Dictionary file path.</param>
        public DictionaryTreeBuilder(string dictionaryFilePath)
        {
            var lines = File.ReadLines(dictionaryFilePath).Where(d => d.Length > 2).Select(d => d.ToLower());
            Dictionary<char, DictionaryTree> tmpNode = null;
            foreach (var line in lines)
            {
                var node = DictionaryTree.Node;
                var lastChar = Char.MinValue;
                foreach (var character in line)
                {
                    if (node.ContainsKey(character))
                    {
                        tmpNode = node;
                        node = node[character].Node;
                        
                    }
                    else
                    {
                        var newNode = new DictionaryTree();
                        node.Add(character, newNode);
                        tmpNode = node;
                        node = newNode.Node;
                    }
                    lastChar = character;
                }
                tmpNode?[lastChar].Words.Add(line);
            }
        }

        /// <summary>
        /// Dictionary tree.
        /// </summary>
        public DictionaryTree DictionaryTree { get; set; } = new DictionaryTree();
    }
}
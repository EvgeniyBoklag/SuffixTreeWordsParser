using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Storage of the dictionary.
    /// </summary>
    
    public class DictionaryStorage
    {
        private HashSet<string> _dictionary;
        public string SmallestWord { get; private set; }
        public short MinWordLength { get; private set; }
        
        /// <summary>
        /// Load dictionary from the text file. Words should be separated by the new line.
        /// </summary>
        /// <param name="path">Path to dictionary text file.</param>
        public void LoadDictionary(string path)
        {
            var lines = File.ReadLines(path).Where(d=>d.Length > 2).Select(d=>d.ToLower());
            MinWordLength = (short)lines.Min(d => d.Length);
            SmallestWord = lines.First(d=>d.Length == MinWordLength);
            _dictionary = new HashSet<string>(lines);
        }

        /// <summary>
        /// Check, if passed word exists in the storage.
        /// </summary>
        /// <param name="word">The search word.</param>
        /// <returns></returns>
        public bool Exists(string word)
        {
            return _dictionary.Contains(word);
        }

    }
    
}

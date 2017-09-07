using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Dictionary tree (suffix tree). 
    /// </summary>
    public class DictionaryTree
    {
        /// <summary>
        /// Key-value collection, where key is a letter.
        /// </summary>
        public Dictionary<char, DictionaryTree> Node { get; set; } = new Dictionary<char, DictionaryTree>();
        
        /// <summary>
        /// Collection of words.
        /// </summary>
        public HashSet<string> Words { get; set; } = new HashSet<string>();
    }
}
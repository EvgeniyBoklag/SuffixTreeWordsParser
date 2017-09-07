using System.Collections.Generic;

namespace Core
{
    public interface IMultipleWordsParser
    {
        Dictionary<string, List<WordsResult>> Result { get; }
        void Parse(string word, string masterWord);
        void SaveParseResult(string path);
    }
}
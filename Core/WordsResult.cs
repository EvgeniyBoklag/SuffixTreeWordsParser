using System.Collections.ObjectModel;

namespace Core
{

    /// <summary>
    /// Finded words result.
    /// </summary>
    public class WordsResult
    {
        public Collection<string> Words { get; set; } = new Collection<string>();
    }
}
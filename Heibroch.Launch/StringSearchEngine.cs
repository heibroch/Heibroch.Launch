using System.Collections.Generic;
using System.Linq;

namespace Heibroch.Launch
{
    public interface IStringSearchEngine
    {
        SortedList<string, string> Search(string searchString, Dictionary<string, string> _shortcuts);
    }

    public class StringSearchEngine : IStringSearchEngine
    {
        public SortedList<string, string> Search(string searchString, Dictionary<string, string> _shortcuts)
        {
            SortedList<string, string> searchResults;
            //It should be an empty list if no query string
            if (string.IsNullOrWhiteSpace(searchString))
            {
                searchResults = new SortedList<string, string>();
            }
            //Redo the list
            else
            {
                searchResults = new SortedList<string, string>(_shortcuts
                    .Where(x => x.Key.ToLower().StartsWith(searchString) || x.Key.ToLower().Contains(searchString))
                    .ToDictionary(z => z.Key, y => y.Value));
            }

            return searchResults;
        }
    }
}

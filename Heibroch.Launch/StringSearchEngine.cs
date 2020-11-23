using System.Collections.Generic;
using System.Linq;

namespace Heibroch.Launch
{
    public interface IStringSearchEngine<T>
    {
        SortedList<string, T> Search(string searchString, Dictionary<string, T> _shortcuts);
    }

    public class StringSearchEngine<T> : IStringSearchEngine<T>
    {
        public SortedList<string, T> Search(string searchString, Dictionary<string, T> _shortcuts)
        {
            searchString = searchString.ToLower();

            SortedList<string, T> searchResults;
            //It should be an empty list if no query string
            if (string.IsNullOrWhiteSpace(searchString))
            {
                searchResults = new SortedList<string, T>();
            }
            //Redo the list
            else
            {
                searchResults = new SortedList<string, T>(_shortcuts
                    .Where(x => x.Key.ToLower().StartsWith(searchString) || x.Key.ToLower().Contains(searchString))
                    .ToDictionary(z => z.Key, y => y.Value));
            }

            return searchResults;
        }
    }
}

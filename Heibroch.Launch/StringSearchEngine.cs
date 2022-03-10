using System.Collections.Generic;
using System.Linq;

namespace Heibroch.Launch
{
    public interface IStringSearchEngine<T>
    {
        List<KeyValuePair<string, T>> Search(string searchString, Dictionary<string, T> _shortcuts, bool useStickySearch);
    }

    public class StringSearchEngine<T> : IStringSearchEngine<T>
    {
        public bool IsStickyMatch(string stringToSearch, string searchString)
        {
            //Run through string and pair up characters along the string to search
            var searchStringIndex = 0;
            for (int stringToSearchIndex = 0; stringToSearchIndex < stringToSearch.Length; stringToSearchIndex++)
            {
                //If there's no match in the current character in the string to the first character in the search string, then progress to
                //the next character in the string we are trying to find matches in
                if (stringToSearch[stringToSearchIndex] != searchString[searchStringIndex])
                {
                    continue;
                }

                //If there's a match, then increment the index
                searchStringIndex++;

                if (searchStringIndex >= searchString.Length)
                    return true;
            }

            return false;
        }

        public List<KeyValuePair<string, T>> Search(string searchString, Dictionary<string, T> shortcuts, bool useStickySearch)
        {
            searchString = searchString.ToLower();

            //It should be an empty list if no query string
            if (string.IsNullOrWhiteSpace(searchString))
                return new List<KeyValuePair<string, T>>();
            
            //Redo the list

            //Add exact matches
            var exactMatches = new List<KeyValuePair<string, T>>();
            var runningMatches = new List<KeyValuePair<string, T>>();

            //Add running matches
            foreach (var shortcut in shortcuts)
            {
                //Is exact match
                if (shortcut.Key.ToLower().Contains(searchString))
                {
                    exactMatches.Add(shortcut);
                    continue;
                }

                //Is sticky match
                if (useStickySearch && IsStickyMatch(shortcut.Key.ToLower(), searchString))
                    runningMatches.Add(shortcut);
            }

            var results = new List<KeyValuePair<string, T>>();
            results.AddRange(exactMatches.OrderBy(x => x.Key));
            results.AddRange(runningMatches.OrderBy(x => x.Key));
            return results;
        }
    }
}

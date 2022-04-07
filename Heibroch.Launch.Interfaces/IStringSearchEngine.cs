namespace Heibroch.Launch.Interfaces
{
    public interface IStringSearchEngine<T>
    {
        List<KeyValuePair<string, T>> Search(string searchString, Dictionary<string, T> shortcuts, bool useStickySearch);
    }
}

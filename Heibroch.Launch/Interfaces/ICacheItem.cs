namespace Heibroch.Launch.Interfaces
{
    public interface ICacheItem<TKey, TTarget>
    {
        TKey Key { get; set; }
        TTarget Target { get; set; }
    }
}

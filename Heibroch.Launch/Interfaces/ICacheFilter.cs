namespace Heibroch.Launch.Interfaces
{
    public interface ICacheFilter<TKey, TTarget>
    {
        TKey Key { get; set; }
    }
}

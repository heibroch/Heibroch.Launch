using Heibroch.Launch.Interfaces;

namespace Heibroch.Launch.Implementation
{
    public class CacheFilter : ICacheFilter<string, string>
    {
        public string Key { get; set; }
    }
}

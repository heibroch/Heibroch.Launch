using Heibroch.Launch.Interfaces;

namespace Heibroch.Launch
{
    public class PathRepository : IPathRepository
    {
        public string AppSettingsDirectory => Constants.RootPath;
    }
}

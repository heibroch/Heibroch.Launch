namespace Heibroch.Launch.Interfaces
{
    public interface IPluginLoader
    {
        List<ILaunchPlugin> Plugins { get; }

        void Load();
    }
}

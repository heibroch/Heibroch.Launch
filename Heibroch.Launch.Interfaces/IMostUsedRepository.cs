namespace Heibroch.Launch.Interfaces
{
    public interface IMostUsedRepository
    {
        List<Tuple<string, int>> ShortcutUseCounts { get; set; }
    }
}

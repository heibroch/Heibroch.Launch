using System;
using System.Collections.Generic;

namespace Heibroch.Launch.Interfaces
{
    public interface ILaunchShortcut
    {
        Action<IEnumerable<KeyValuePair<string, string>>, string> Execute { get; }

        string Title { get; }

        string Description { get; }
    }
}

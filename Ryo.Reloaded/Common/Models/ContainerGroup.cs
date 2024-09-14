using Ryo.Interfaces.Classes;
using Ryo.Reloaded.Common.Models;

namespace Ryo.Reloaded.Audio.Models;

internal class ContainerGroup(string id, IContainer[] containers) : IContainerGroup
{
    public string Id { get; } = id;

    public void Enable()
    {
        foreach (var container in containers)
        {
            container.IsEnabled = true;
        }
    }

    public void Disable()
    {
        foreach (var container in containers)
        {
            container.IsEnabled = false;
        }
    }
}

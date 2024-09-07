using Ryo.Interfaces.Classes;
using Ryo.Reloaded.Audio.Models.Containers;

namespace Ryo.Reloaded.Audio.Models;

internal class ContainerGroup(string id, BaseContainer[] containers) : IContainerGroup
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

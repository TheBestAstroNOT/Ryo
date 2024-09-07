using Ryo.Interfaces.Classes;
using Ryo.Reloaded.Audio.Models.Containers;

namespace Ryo.Reloaded.Audio.Models;

internal class ContainerGroup(BaseContainer[] containers) : IContainerGroup
{
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

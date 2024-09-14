namespace Ryo.Reloaded.Common.Models;

internal interface IContainer
{
    public bool IsEnabled { get; set; }

    public string? GroupId { get; }

    public string? SharedContainerId { get; }
}

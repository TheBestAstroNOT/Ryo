namespace Ryo.Interfaces.Classes;

/// <summary>
/// Container group interface.
/// </summary>
public interface IContainerGroup
{
    /// <summary>
    /// Gets container group ID.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Enables all containers in group.
    /// </summary>
    void Enable();

    /// <summary>
    /// Disables all containers in group.
    /// </summary>
    void Disable();
}

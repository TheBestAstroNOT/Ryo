namespace Ryo.Interfaces.Classes;

/// <summary>
/// Movie configuration.
/// </summary>
public class MovieConfig
{
    /// <summary>
    /// Gets or sets the container group.
    /// Settings this allows for the container to be retrieved
    /// as part of a group of containers.
    /// </summary>
    public string? GroupId { get; set; }

    public bool? IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the shared container ID.
    /// Setting this allows for multiple movies to be added
    /// to the same movies container.
    /// </summary>
    public string? SharedContainerId { get; set; }

    /// <summary>
    /// Gets or sets the target movie path to replace.
    /// </summary>
    public string? TargetMoviePath { get; set; }

    /// <summary>
    /// Gets or sets the movie path.
    /// </summary>
    public string? MoviePath { get; set; }

    public MovieConfig Clone() => (MovieConfig)this.MemberwiseClone();

    /// <summary>
    /// Applies defined settings from <paramref name="newConfig"/>.
    /// </summary>
    /// <param name="newConfig">Config containing the settings to apply.</param>
    public void Apply(MovieConfig newConfig)
    {
        this.GroupId = newConfig.GroupId ?? this.GroupId;
        this.IsEnabled = newConfig.IsEnabled ?? this.IsEnabled;
        this.SharedContainerId = newConfig.SharedContainerId ?? this.SharedContainerId;
        this.TargetMoviePath = newConfig.TargetMoviePath ?? this.TargetMoviePath;
        this.MoviePath = newConfig.MoviePath ?? this.MoviePath;
    }
}

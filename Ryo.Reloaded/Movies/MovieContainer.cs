using Ryo.Interfaces.Classes;
using Ryo.Reloaded.Common.Models;

namespace Ryo.Reloaded.Movies;

internal class MovieContainer : IContainer
{
    private readonly List<string> moviePaths = [];

    public MovieContainer(MovieConfig? config)
    {
        this.GroupId = config?.GroupId;
        this.IsEnabled = config?.IsEnabled ?? true;
        this.SharedContainerId = config?.SharedContainerId;
        this.TargetMoviePath = config?.TargetMoviePath ?? throw new Exception("Missing target movie path.");
    }

    public bool IsEnabled { get; set; }

    public string? GroupId { get; }

    public string? SharedContainerId { get; }

    public string TargetMoviePath { get; }

    public void AddMovie(string newMoviePath)
    {
        this.moviePaths.Add(newMoviePath);
        Log.Information($"Added new movie to: {this.TargetMoviePath}\nMovie: {newMoviePath}");
    }

    public string GetMoviePath()
    {
        if (this.moviePaths.Count > 1)
        {
            var randomIndex = Random.Shared.Next(0, this.moviePaths.Count);
            Log.Debug($"Random movie index: {randomIndex} || Total Files: {this.moviePaths.Count}");
            return this.moviePaths[randomIndex];
        }
        else if (this.moviePaths.Count == 1)
        {
            return this.moviePaths[0];
        }

        throw new Exception($"Movie had no files.\nOriginal File: {this.TargetMoviePath}");
    }
}

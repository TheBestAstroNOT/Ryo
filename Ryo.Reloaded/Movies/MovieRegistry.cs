using Ryo.Interfaces.Classes;
using Ryo.Reloaded.Common;
using Ryo.Reloaded.Common.Models;
using System.Diagnostics.CodeAnalysis;

namespace Ryo.Reloaded.Movies;

internal class MovieRegistry
{
    private readonly Dictionary<string, List<MovieContainer>> containers = new(StringComparer.OrdinalIgnoreCase);

    public void AddMoviePath(string path, MovieConfig? config)
    {
        if (Directory.Exists(path))
        {
            this.AddMovieFolder(path, config);
        }
        else if (File.Exists(path))
        {
            this.AddMovieFile(path, config);
        }
        else
        {
            Log.Error($"Movie path was not found.\nPath: {path}");
        }
    }

    private void AddMovieFolder(string dir, MovieConfig? preConfig)
    {
        Log.Information($"Adding movie folder: {dir}");

        var config = preConfig?.Clone() ?? new();

        // Apply config from a folder config file.
        var dirConfigFile = Path.Join(dir, "config.yaml");
        if (File.Exists(dirConfigFile) && ParseConfigFile(dirConfigFile) is MovieConfig dirConfig)
        {
            config.Apply(dirConfig);
        }

        // Folder likely contains items for a single item/file,
        // and thus items should be added to a single container.
        if (dir.EndsWith(".usm", StringComparison.OrdinalIgnoreCase))
        {
            config.SharedContainerId = Guid.NewGuid().ToString();
        }

        foreach (var file in Directory.EnumerateFiles(dir))
        {
            this.AddMovieFile(file, config);
        }

        foreach (var folder in Directory.EnumerateDirectories(dir))
        {
            this.AddMovieFolder(folder, config);
        }
    }

    public void AddMoviePath(string path) => this.AddMoviePath(path, null);

    public void AddMovieBind(string targetMoviePath, string bindPath)
    {
        var container = this.RegisterOrGetSharedContainer(new(new() { TargetMoviePath = targetMoviePath }));
        container.AddMovie(bindPath);
    }

    public bool TryGetMovie(string targetMoviePath, [NotNullWhen(true)] out MovieContainer? container)
    {
        if (this.containers.TryGetValue(targetMoviePath, out var containerList))
        {
            container = containerList.LastOrDefault(x => x.IsEnabled);
            return container != null;
        }

        container = null;
        return false;
    }

    public IContainer[] GetContainersByGroup(string groupId)
    {
        var containers = this.containers.Values.SelectMany(x => x)
            .Where(x => x.GroupId != null && x.GroupId == groupId)
            .ToArray();
        return containers;
    }

    private static MovieConfig? ParseConfigFile(string configFile)
    {
        try
        {
            Log.Debug($"Loading movie config: {configFile}");
            var config = YamlSerializer.DeserializeFile<MovieConfig>(configFile);
            return config;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to add movie config.\nFile: {configFile}");
            return null;
        }
    }

    private void AddMovieFile(string file, MovieConfig? preConfig)
    {
        var config = preConfig?.Clone() ?? new();

        // Apply config settings from file config.
        var configFile = Path.ChangeExtension(file, ".yaml");
        if (File.Exists(configFile) && ParseConfigFile(configFile) is MovieConfig fileConfig)
        {
            config.Apply(fileConfig);
        }

        config.TargetMoviePath ??= Path.GetFileName(file);

        var container = this.RegisterOrGetSharedContainer(new(config));
        if (config.MoviePath != null)
        {
            container.AddMovie(config.MoviePath);
        }
        else if (Path.GetExtension(file).Equals(".usm", StringComparison.OrdinalIgnoreCase))
        {
            container.AddMovie(file);
        }
    }

    private MovieContainer RegisterOrGetSharedContainer(MovieContainer container)
    {
        var moviePath = container.TargetMoviePath;
        if (this.containers.ContainsKey(moviePath) == false)
        {
            containers[moviePath] = [container];
            return container;
        }

        if (container.SharedContainerId != null)
        {
            var sharedContainer = containers[moviePath].FirstOrDefault(x => x.SharedContainerId == container.SharedContainerId);
            if (sharedContainer != null)
            {
                return sharedContainer;
            }
        }

        containers[moviePath].Add(container);
        return container;
    }
}

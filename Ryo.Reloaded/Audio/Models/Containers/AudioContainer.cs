using Ryo.Interfaces.Classes;
using Ryo.Interfaces.Enums;
using Ryo.Reloaded.Common.Models;

namespace Ryo.Reloaded.Audio.Models.Containers;

internal abstract class AudioContainer : IContainer
{
    private readonly PlaybackMode playMode;
    private readonly List<RyoAudio> audios = [];

    private int prevIndex = -1;

    public AudioContainer(AudioConfig? config)
    {
        this.playMode = config?.PlaybackMode ?? PlaybackMode.Random;

        this.GroupId = config?.GroupId;
        this.IsEnabled = config?.IsEnabled ?? true;
        this.PlayerId = config?.PlayerId ?? -1;
        this.CategoryIds = config?.CategoryIds;
        this.SharedContainerId = config?.SharedContainerId;
    }

    public bool IsEnabled { get; set; }

    public string? GroupId { get; }

    public string? SharedContainerId { get; }

    public abstract string Name { get; }

    public int PlayerId { get; }

    public int[]? CategoryIds { get; }

    public void AddAudio(RyoAudio audio)
    {
        audios.Add(audio);
        Log.Information($"{this.Name}\nFile added: {audio.FilePath}");
    }

    public RyoAudio GetAudio()
    {
        if (audios.Count > 1)
        {
            if (this.playMode == PlaybackMode.Random)
            {
                var randomIndex = Random.Shared.Next(0, audios.Count);
                if (randomIndex == this.prevIndex)
                {
                    randomIndex = (randomIndex + 1) % this.audios.Count;
                    this.prevIndex = randomIndex;
                }

                Log.Debug($"{this.Name} || Random Index: {randomIndex} || Total Files: {this.audios.Count}");
                return audios[randomIndex];
            }
            else
            {
                var nextIndex = (this.prevIndex + 1) % this.audios.Count;
                this.prevIndex = nextIndex;
                Log.Debug($"{this.Name} || Next Index: {nextIndex} || Total Files: {this.audios.Count}");
                return audios[nextIndex];
            }
        }
        else if (audios.Count == 1)
        {
            return audios[0];
        }

        throw new Exception($"Audio had no files.\n{Name}");
    }

    public string[] GetContainerFiles() => audios.Select(x => x.FilePath).ToArray();
}

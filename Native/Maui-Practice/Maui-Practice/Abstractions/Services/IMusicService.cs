using Maui_Practice.Models;

namespace Maui_Practice.Abstractions.Services;
public interface IMusicService
{
    Task<List<MusicFile>> GetLocalMusicFilesAsync();
    Task<List<MusicFile>> SearchOnlineMusicAsync(string query);
    Task<bool> DownloadMusicAsync(MusicFile musicFile, string url);
    Task<bool> PlayMusicAsync(MusicFile musicFile);
    Task<bool> DeleteLocalMusicAsync(MusicFile musicFile);
}


using Maui_Practice.Models;

namespace Maui_Practice.Abstractions.Services;
public interface IDatabaseService
{
    Task InitializeDatabase();
    Task<List<MusicFile>> GetMusicFilesAsync();
    Task<MusicFile> GetMusicFileAsync(int id);
    Task<int> SaveMusicFileAsync(MusicFile musicFile);
    Task<int> DeleteMusicFileAsync(MusicFile musicFile);

    Task<List<FavoriteMusic>> GetFavoritesAsync();
    Task<bool> IsFavoriteAsync(int musicFileId);
    Task<int> AddToFavoritesAsync(int musicFileId);
    Task<int> RemoveFromFavoritesAsync(int musicFileId);

    Task<AppSettings> GetAppSettingsAsync();
    Task<int> SaveAppSettingsAsync(AppSettings settings);
}
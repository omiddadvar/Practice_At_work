using Maui_Practice.Abstractions.Services;
using Maui_Practice.Models;
using Microsoft.VisualBasic;
using SQLite;

namespace Maui_Practice.Services;
public class DatabaseService : IDatabaseService
{
    private SQLiteAsyncConnection _database;

    public async Task InitializeDatabase()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

        await _database.CreateTableAsync<MusicFile>();
        await _database.CreateTableAsync<FavoriteMusic>();
        await _database.CreateTableAsync<AppSettings>();

        // Initialize default settings
        var settings = await GetAppSettingsAsync();
        if (settings == null)
        {
            await SaveAppSettingsAsync(new AppSettings());
        }
    }

    public async Task<List<MusicFile>> GetMusicFilesAsync()
    {
        await InitializeDatabase();
        return await _database.Table<MusicFile>().ToListAsync();
    }

    public async Task<MusicFile> GetMusicFileAsync(int id)
    {
        await InitializeDatabase();
        return await _database.Table<MusicFile>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveMusicFileAsync(MusicFile musicFile)
    {
        await InitializeDatabase();
        return musicFile.Id != 0
            ? await _database.UpdateAsync(musicFile)
            : await _database.InsertAsync(musicFile);
    }

    public async Task<int> DeleteMusicFileAsync(MusicFile musicFile)
    {
        await InitializeDatabase();
        return await _database.DeleteAsync(musicFile);
    }

    public async Task<List<FavoriteMusic>> GetFavoritesAsync()
    {
        await InitializeDatabase();
        return await _database.Table<FavoriteMusic>().ToListAsync();
    }

    public async Task<bool> IsFavoriteAsync(int musicFileId)
    {
        await InitializeDatabase();
        var favorite = await _database.Table<FavoriteMusic>()
            .Where(x => x.MusicFileId == musicFileId)
            .FirstOrDefaultAsync();
        return favorite != null;
    }

    public async Task<int> AddToFavoritesAsync(int musicFileId)
    {
        await InitializeDatabase();
        var favorite = new FavoriteMusic { MusicFileId = musicFileId };
        return await _database.InsertAsync(favorite);
    }

    public async Task<int> RemoveFromFavoritesAsync(int musicFileId)
    {
        await InitializeDatabase();
        var favorite = await _database.Table<FavoriteMusic>()
            .Where(x => x.MusicFileId == musicFileId)
            .FirstOrDefaultAsync();

        return favorite != null ? await _database.DeleteAsync(favorite) : 0;
    }

    public async Task<AppSettings> GetAppSettingsAsync()
    {
        await InitializeDatabase();
        return await _database.Table<AppSettings>().FirstOrDefaultAsync();
    }

    public async Task<int> SaveAppSettingsAsync(AppSettings settings)
    {
        await InitializeDatabase();
        return settings.Id != 0
            ? await _database.UpdateAsync(settings)
            : await _database.InsertAsync(settings);
    }
}
using Maui_Practice.Abstractions.Services;
using Maui_Practice.Constants;
using Maui_Practice.Models;
using Microsoft.VisualBasic;

namespace Maui_Practice.Services;
public class MusicService : IMusicService
{
    private readonly IDatabaseService _databaseService;

    public MusicService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<MusicFile>> GetLocalMusicFilesAsync()
    {
        try
        {
            // Get music from local storage
            var localFiles = await ScanLocalMusicFiles();

            // Also get from database
            var dbFiles = await _databaseService.GetMusicFilesAsync();

            // Merge results (you might want more sophisticated merging logic)
            return dbFiles.UnionBy(localFiles, x => x.FilePath).ToList();
        }
        catch (Exception ex)
        {
            // Handle exception
            return new List<MusicFile>();
        }
    }

    private async Task<List<MusicFile>> ScanLocalMusicFiles()
    {
        var musicFiles = new List<MusicFile>();

        try
        {
            // For Android - check Downloads and Music folders
            var downloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Downloads");
            var musicFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));

            var folders = new List<string> { downloadFolder, musicFolder };

            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder)) continue;

                var files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
                    .Where(file => DeviceRelatedConstants.SupportedFormats.Any(format =>
                        file.EndsWith(format, StringComparison.OrdinalIgnoreCase)));

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    var musicFile = new MusicFile
                    {
                        Title = Path.GetFileNameWithoutExtension(file),
                        Artist = "Unknown Artist",
                        Album = "Unknown Album",
                        FilePath = file,
                        FileName = Path.GetFileName(file),
                        FileSize = fileInfo.Length,
                        DateAdded = fileInfo.CreationTime,
                        IsDownloaded = true
                    };

                    musicFiles.Add(musicFile);

                    // Save to database if not exists
                    var existing = (await _databaseService.GetMusicFilesAsync())
                        .FirstOrDefault(m => m.FilePath == file);

                    if (existing == null)
                    {
                        await _databaseService.SaveMusicFileAsync(musicFile);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle permission or other exceptions
            System.Diagnostics.Debug.WriteLine($"Error scanning files: {ex.Message}");
        }

        return musicFiles;
    }

    public async Task<List<MusicFile>> SearchOnlineMusicAsync(string query)
    {
        // Mock implementation - replace with actual API call
        await Task.Delay(500); // Simulate network delay

        return new List<MusicFile>
        {
            new MusicFile
            {
                Title = $"{query} Song 1",
                Artist = "Online Artist 1",
                Album = "Online Album",
                OnlineId = Guid.NewGuid().ToString(),
                IsDownloaded = false
            },
            new MusicFile
            {
                Title = $"{query} Song 2",
                Artist = "Online Artist 2",
                Album = "Online Album",
                OnlineId = Guid.NewGuid().ToString(),
                IsDownloaded = false
            }
        };
    }

    public async Task<bool> DownloadMusicAsync(MusicFile musicFile, string url)
    {
        try
        {
            // Mock download - replace with actual download logic
            await Task.Delay(2000);

            var settings = await _databaseService.GetAppSettingsAsync();
            var downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                          settings?.DownloadPath ?? "Music");

            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }

            var filePath = Path.Combine(downloadPath, $"{musicFile.Title}.mp3");

            // In real implementation, download from URL and save to filePath
            // For now, we'll just create the file entry
            musicFile.FilePath = filePath;
            musicFile.FileName = Path.GetFileName(filePath);
            musicFile.IsDownloaded = true;
            musicFile.DateAdded = DateTime.Now;

            await _databaseService.SaveMusicFileAsync(musicFile);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> PlayMusicAsync(MusicFile musicFile)
    {
        // MAUI's MediaElement would handle playback
        // This is just a placeholder for playback logic
        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteLocalMusicAsync(MusicFile musicFile)
    {
        try
        {
            if (File.Exists(musicFile.FilePath))
            {
                File.Delete(musicFile.FilePath);
            }

            await _databaseService.DeleteMusicFileAsync(musicFile);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
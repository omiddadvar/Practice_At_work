
using System.ComponentModel.DataAnnotations;

namespace Maui_Practice.Models;
public class MusicFile
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public long FileSize { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.Now;
    public bool IsDownloaded { get; set; }
    public string OnlineId { get; set; } = string.Empty;
}

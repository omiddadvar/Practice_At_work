

using System.ComponentModel.DataAnnotations;

namespace Maui_Practice.Models;
public class AppSettings
{
    [Key]
    public int Id { get; set; }
    public bool IsDarkMode { get; set; } = true;
    public string DownloadPath { get; set; } = "Music";
    public bool AutoPlay { get; set; } = true;
    public string ThemeColor { get; set; } = "#6750A4";
}
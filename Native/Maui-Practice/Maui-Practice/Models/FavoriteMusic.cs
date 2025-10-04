

using System.ComponentModel.DataAnnotations;

namespace Maui_Practice.Models;
public class FavoriteMusic
{
    [Key]
    public int Id { get; set; }
    public int MusicFileId { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.Now;
}
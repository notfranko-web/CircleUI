namespace CircleUI.Models;

public class Asset
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = "image/png";
    public long SizeBytes { get; set; }
    public string StoragePath { get; set; } = string.Empty;
    
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
}
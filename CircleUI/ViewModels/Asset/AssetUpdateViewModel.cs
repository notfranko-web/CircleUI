namespace CircleUI.ViewModels.Asset;

public class AssetUpdateViewModel
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = "image/png";
    public long SizeBytes { get; set; }
    public string StoragePath { get; set; } = string.Empty;
    public string UserId { get; set; } = null!;
}
namespace CircleUI.ViewModels.Asset;

public class AssetIndexViewModel
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = "image/png";
    public long SizeBytes { get; set; }
    public string UserId { get; set; } = null!;
}
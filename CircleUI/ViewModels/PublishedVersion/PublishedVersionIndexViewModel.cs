namespace CircleUI.ViewModels.PublishedVersion;

public class PublishedVersionIndexViewModel
{
    public Guid Id { get; set; }
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public string VersionHash { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
}
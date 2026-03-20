namespace CircleUI.Core.DTOs;

public class PublishedVersionDTO
{
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public string VersionHash { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
}

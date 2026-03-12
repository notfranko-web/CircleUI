namespace CircleUI.Data.Models;

public class PublishedVersion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public string VersionHash { get; set; } = string.Empty;
    
    public Guid ProjectId { get; set; }
    public WebsiteProject Project { get; set; } = null!;
}
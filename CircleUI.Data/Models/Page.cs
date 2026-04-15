namespace CircleUI.Data.Models;

public class Page
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "New Page";
    public string Path { get; set; } = "/"; // e.g., "/about"
    public string MetaDescription { get; set; } = string.Empty;
    public string MetaKeywords { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid ProjectId { get; set; }
    public WebsiteProject Project { get; set; } = null!;
    
    public ICollection<PageSection> PageSections { get; set; } = new List<PageSection>();
}
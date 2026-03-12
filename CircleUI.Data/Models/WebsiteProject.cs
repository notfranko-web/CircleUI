namespace CircleUI.Data.Models;

public class WebsiteProject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "New Project";
    public string Description { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    
    public ICollection<Page> Pages { get; set; } = new List<Page>();
    public ICollection<PublishedVersion> PublishedVersions { get; set; } = new List<PublishedVersion>();
}
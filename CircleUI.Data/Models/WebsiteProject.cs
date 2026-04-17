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
    
    public string BackgroundColor { get; set; } = "#ffffff";
    public string PrimaryTextColor { get; set; } = "#000000";
    public string SecondaryTextColor { get; set; } = "#6c757d";
    public string ButtonColor { get; set; } = "#0d6efd";
    public string ButtonTextColor { get; set; } = "#ffffff";
    public string? BackgroundImage { get; set; }
    
    public ICollection<Page> Pages { get; set; } = new List<Page>();
    public ICollection<PublishedVersion> PublishedVersions { get; set; } = new List<PublishedVersion>();
}
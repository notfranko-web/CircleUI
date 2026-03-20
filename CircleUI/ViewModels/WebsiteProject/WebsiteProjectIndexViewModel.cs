namespace CircleUI.ViewModels.WebsiteProject;

public class WebsiteProjectIndexViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "New Project";
    public string Domain { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public string UserId { get; set; } = null!;
}
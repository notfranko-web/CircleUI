namespace CircleUI.ViewModels.WebsiteProject;

public class WebsiteProjectUpdateViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "New Project";
    public string Description { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public string UserId { get; set; } = null!;
}
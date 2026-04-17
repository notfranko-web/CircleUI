namespace CircleUI.Core.DTOs;

public class WebsiteProjectDTO
{
    public Guid? Id { get; set; } = null;
    public string Name { get; set; } = "New Project";
    public string Description { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public string UserId { get; set; } = null!;
    public string BackgroundColor { get; set; } = "#ffffff";
    public string PrimaryTextColor { get; set; } = "#000000";
    public string SecondaryTextColor { get; set; } = "#6c757d";
    public string ButtonColor { get; set; } = "#0d6efd";
    public string ButtonTextColor { get; set; } = "#ffffff";
    public List<PageDTO> Pages { get; set; } = new List<PageDTO>();
}

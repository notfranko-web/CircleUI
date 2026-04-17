using CircleUI.Core.DTOs;

namespace CircleUI.ViewModels.WebsiteProject;

public class WebsiteProjectBuilderViewModel
{
    public Guid Id { get; set; }
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
    public string? BackgroundImage { get; set; }
    public List<ComponentDTO> ComponentDtos = new List<ComponentDTO>();
    public List<PageDTO> PageDtos = new List<PageDTO>();
    public string? ActiveTab { get; set; }
}
namespace CircleUI.Core.DTOs;

public class WebsiteProjectDTO
{
    public Guid? Id { get; set; } = null;
    public string Name { get; set; } = "New Project";
    public string Description { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public string UserId { get; set; } = null!;
    public List<PageDTO> Pages { get; set; } = new List<PageDTO>();
}

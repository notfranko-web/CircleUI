namespace CircleUI.Core.DTOs;

public class PageDTO
{
    // Should this be here? (applies for the rest of the DTOs)
    public Guid? Id { get; set; }
    public string Title { get; set; } = "New Page";
    public string Path { get; set; } = "/";
    public string MetaDescription { get; set; } = string.Empty;
    public string MetaKeywords { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
    public List<SectionDTO> SectionDtos = new List<SectionDTO>();
}

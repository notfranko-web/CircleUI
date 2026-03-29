namespace CircleUI.Core.DTOs;

public class SectionDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "New Section";
    public string HTMLId { get; set; } = string.Empty;
    public string CSSClass { get; set; } = string.Empty;
    
    public ICollection<SectionComponentDTO> SectionComponentDTOs { get; set; } = new List<SectionComponentDTO>();
    public ICollection<ComponentDTO> ComponentDTOs { get; set; } = new List<ComponentDTO>();
    public ICollection<PageSectionDTO> PageSectionsDTOs { get; set; } = new List<PageSectionDTO>();
}
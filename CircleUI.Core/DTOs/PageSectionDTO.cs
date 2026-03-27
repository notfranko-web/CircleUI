namespace CircleUI.Core.DTOs;

public class PageSectionDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PageId { get; set; }
    public Guid SectionId { get; set; }
    public int Order { get; set; }
}
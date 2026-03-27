namespace CircleUI.Core.DTOs;

public class SectionComponentDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SectionId { get; set; }
    public Guid ComponentId { get; set; }
    public int Order { get; set; }
}
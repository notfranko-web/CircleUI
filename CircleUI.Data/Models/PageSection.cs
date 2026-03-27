namespace CircleUI.Data.Models;

public class PageSection
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PageId { get; set; }
    public Guid SectionId { get; set; }
    public int Order { get; set; }
    public Section Section { get; set; } 
}
namespace CircleUI.Data.Models;

public class Section
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "New Section";
    public string HTMLId { get; set; } = string.Empty;
    public string CSSClass { get; set; } = string.Empty;
    
    public ICollection<SectionComponent> SectionComponents { get; set; } = new List<SectionComponent>();
    public ICollection<PageSection> PageSections { get; set; } = new List<PageSection>();
}
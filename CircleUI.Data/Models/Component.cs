namespace CircleUI.Data.Models;

public class Component
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; } = "Text";
    public string Category { get; set; } = "General";
    public string Content { get; set; } = string.Empty;

    public string Layout { get; set; } = "{\"desktop\":{\"x\":0,\"y\":0,\"w\":12},\"tablet\":{\"x\":0,\"y\":0,\"w\":12},\"mobile\":{\"x\":0,\"y\":0,\"w\":12}}";
    
    public ICollection<SectionComponent> SectionComponents { get; set; } = new List<SectionComponent>();
}
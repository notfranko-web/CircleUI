namespace CircleUI.Data.Models;

public class Component
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; } = "Text";
    public string Content { get; set; } = string.Empty; 
    
    public string Layout { get; set; } = "{\"desktop\":{\"x\":0,\"y\":0,\"w\":12},\"tablet\":{\"x\":0,\"y\":0,\"w\":12},\"mobile\":{\"x\":0,\"y\":0,\"w\":12}}";
    
    public Guid? ParentId { get; set; }
    public Component? Parent { get; set; }
    public ICollection<Component> Children { get; set; } = new List<Component>();
    
    public Guid PageId { get; set; }
    public Page Page { get; set; } = null!;
}
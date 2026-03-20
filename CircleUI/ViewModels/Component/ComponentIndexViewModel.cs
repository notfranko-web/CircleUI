namespace CircleUI.ViewModels.Component;

public class ComponentIndexViewModel
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "Text";
    public string Content { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public Guid PageId { get; set; }
}
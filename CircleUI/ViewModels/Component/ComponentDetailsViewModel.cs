namespace CircleUI.ViewModels.Component;

public class ComponentDetailsViewModel
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "Text";
    public string Content { get; set; } = string.Empty;
    public string Layout { get; set; } = "{\"desktop\":{\"x\":0,\"y\":0,\"w\":12},\"tablet\":{\"x\":0,\"y\":0,\"w\":12},\"mobile\":{\"x\":0,\"y\":0,\"w\":12}}";
    public Guid? ParentId { get; set; }
    public Guid PageId { get; set; }
}
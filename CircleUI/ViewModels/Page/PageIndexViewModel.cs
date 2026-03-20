namespace CircleUI.ViewModels.Page;

public class PageIndexViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "New Page";
    public string Path { get; set; } = "/";
    public Guid ProjectId { get; set; }
}
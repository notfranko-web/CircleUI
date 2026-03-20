namespace CircleUI.ViewModels.Page;

public class PageCreateViewModel
{
    public string Title { get; set; } = "New Page";
    public string Path { get; set; } = "/";
    public string MetaDescription { get; set; } = string.Empty;
    public string MetaKeywords { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
}
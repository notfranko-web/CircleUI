namespace CircleUI.Data.Models;

public class SectionComponent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SectionId { get; set; }
    public Guid ComponentId { get; set; }
    public int Order { get; set; }
    public Component Component { get; set; } // REQUIRED
}
using Microsoft.AspNetCore.Identity;

namespace CircleUI.Data.Models;

public class User : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
    public ICollection<WebsiteProject> Projects { get; set; } = new List<WebsiteProject>();
}
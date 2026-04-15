using CircleUI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CircleUI.Data.Seeders;

public static class BootstrapComponentSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Components.AnyAsync(c => c.Category == "Alerts"))
            return;

        var components = new List<Component>
        {
            // ── Alerts ──────────────────────────────────────────────────────────────
            C("Alert", "Alerts", "<div class=\"alert alert-primary\" role=\"alert\">A simple primary alert.</div>"),
            C("Alert", "Alerts", "<div class=\"alert alert-success\" role=\"alert\">A simple success alert.</div>"),
            C("Alert", "Alerts", "<div class=\"alert alert-warning\" role=\"alert\">A simple warning alert.</div>"),
            C("Alert", "Alerts", "<div class=\"alert alert-danger\" role=\"alert\">A simple danger alert.</div>"),
            C("Alert", "Alerts", "<div class=\"alert alert-info\" role=\"alert\">A simple info alert.</div>"),

            // ── Badges ──────────────────────────────────────────────────────────────
            C("Badge", "Badges", "<span class=\"badge text-bg-primary\">Primary</span>"),
            C("Badge", "Badges", "<span class=\"badge text-bg-success\">Success</span>"),
            C("Badge", "Badges", "<span class=\"badge text-bg-danger\">Danger</span>"),
            C("Badge", "Badges", "<span class=\"badge rounded-pill text-bg-secondary\">Pill</span>"),

            // ── Breadcrumb ───────────────────────────────────────────────────────────
            C("Breadcrumb", "Breadcrumb",
                "<nav aria-label=\"breadcrumb\">" +
                "<ol class=\"breadcrumb\">" +
                "<li class=\"breadcrumb-item\"><a href=\"#\">Home</a></li>" +
                "<li class=\"breadcrumb-item\"><a href=\"#\">Library</a></li>" +
                "<li class=\"breadcrumb-item active\" aria-current=\"page\">Data</li>" +
                "</ol></nav>"),

            // ── Buttons ─────────────────────────────────────────────────────────────
            C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-primary\">Primary</button>"),
            C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-secondary\">Secondary</button>"),
            C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-success\">Success</button>"),
            C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-danger\">Danger</button>"),
            C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-outline-primary\">Outline Primary</button>"),
            C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-lg btn-primary\">Large Button</button>"),
            C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-sm btn-primary\">Small Button</button>"),

            // ── Cards ────────────────────────────────────────────────────────────────
            C("Card", "Cards",
                "<div class=\"card\" style=\"width:18rem\">" +
                "<div class=\"card-body\">" +
                "<h5 class=\"card-title\">Card Title</h5>" +
                "<p class=\"card-text\">Some quick example text.</p>" +
                "<a href=\"#\" class=\"btn btn-primary\">Go somewhere</a>" +
                "</div></div>"),
            C("Card", "Cards",
                "<div class=\"card\" style=\"width:18rem\">" +
                "<div class=\"card-header\">Featured</div>" +
                "<div class=\"card-body\">" +
                "<h5 class=\"card-title\">Special title treatment</h5>" +
                "<p class=\"card-text\">With supporting text below.</p>" +
                "<a href=\"#\" class=\"btn btn-primary\">Go somewhere</a>" +
                "</div>" +
                "<div class=\"card-footer text-body-secondary\">2 days ago</div>" +
                "</div>"),

            // ── List Groups ──────────────────────────────────────────────────────────
            C("List Group", "List Groups",
                "<ul class=\"list-group\">" +
                "<li class=\"list-group-item\">An item</li>" +
                "<li class=\"list-group-item\">A second item</li>" +
                "<li class=\"list-group-item\">A third item</li>" +
                "</ul>"),
            C("List Group", "List Groups",
                "<ul class=\"list-group\">" +
                "<li class=\"list-group-item active\">Active item</li>" +
                "<li class=\"list-group-item list-group-item-success\">Success item</li>" +
                "<li class=\"list-group-item list-group-item-danger\">Danger item</li>" +
                "</ul>"),

            // ── Navs ─────────────────────────────────────────────────────────────────
            C("Nav", "Navs",
                "<ul class=\"nav\">" +
                "<li class=\"nav-item\"><a class=\"nav-link active\" href=\"#\">Active</a></li>" +
                "<li class=\"nav-item\"><a class=\"nav-link\" href=\"#\">Link</a></li>" +
                "<li class=\"nav-item\"><a class=\"nav-link disabled\">Disabled</a></li>" +
                "</ul>"),
            C("Nav", "Navs",
                "<ul class=\"nav nav-pills\">" +
                "<li class=\"nav-item\"><a class=\"nav-link active\" href=\"#\">Active</a></li>" +
                "<li class=\"nav-item\"><a class=\"nav-link\" href=\"#\">Link</a></li>" +
                "<li class=\"nav-item\"><a class=\"nav-link\" href=\"#\">Link</a></li>" +
                "</ul>"),

            // ── Navbar ───────────────────────────────────────────────────────────────
            C("Navbar", "Navbar",
                "<nav class=\"navbar navbar-expand-lg bg-body-tertiary\">" +
                "<div class=\"container-fluid\">" +
                "<a class=\"navbar-brand\" href=\"#\">Brand</a>" +
                "<div class=\"collapse navbar-collapse\">" +
                "<ul class=\"navbar-nav me-auto\">" +
                "<li class=\"nav-item\"><a class=\"nav-link active\" href=\"#\">Home</a></li>" +
                "<li class=\"nav-item\"><a class=\"nav-link\" href=\"#\">About</a></li>" +
                "</ul></div></div></nav>"),

            // ── Progress ─────────────────────────────────────────────────────────────
            C("Progress", "Progress",
                "<div class=\"progress\" role=\"progressbar\" aria-valuenow=\"50\" aria-valuemin=\"0\" aria-valuemax=\"100\">" +
                "<div class=\"progress-bar\" style=\"width:50%\">50%</div></div>"),
            C("Progress", "Progress",
                "<div class=\"progress\" role=\"progressbar\" aria-valuenow=\"75\" aria-valuemin=\"0\" aria-valuemax=\"100\">" +
                "<div class=\"progress-bar bg-success\" style=\"width:75%\">75%</div></div>"),

            // ── Spinners ─────────────────────────────────────────────────────────────
            C("Spinner", "Spinners", "<div class=\"spinner-border\" role=\"status\"><span class=\"visually-hidden\">Loading...</span></div>"),
            C("Spinner", "Spinners", "<div class=\"spinner-border text-primary\" role=\"status\"><span class=\"visually-hidden\">Loading...</span></div>"),
            C("Spinner", "Spinners", "<div class=\"spinner-grow\" role=\"status\"><span class=\"visually-hidden\">Loading...</span></div>"),

            // ── Tables ───────────────────────────────────────────────────────────────
            C("Table", "Tables",
                "<table class=\"table\">" +
                "<thead><tr><th>#</th><th>First</th><th>Last</th></tr></thead>" +
                "<tbody>" +
                "<tr><td>1</td><td>Mark</td><td>Otto</td></tr>" +
                "<tr><td>2</td><td>Jacob</td><td>Thornton</td></tr>" +
                "</tbody></table>"),
            C("Table", "Tables",
                "<table class=\"table table-striped table-hover\">" +
                "<thead><tr><th>#</th><th>First</th><th>Last</th></tr></thead>" +
                "<tbody>" +
                "<tr><td>1</td><td>Mark</td><td>Otto</td></tr>" +
                "<tr><td>2</td><td>Jacob</td><td>Thornton</td></tr>" +
                "</tbody></table>"),

            // ── Typography ───────────────────────────────────────────────────────────
            C("Heading", "Typography", "<h1>Heading 1</h1>"),
            C("Heading", "Typography", "<h2>Heading 2</h2>"),
            C("Heading", "Typography", "<h3>Heading 3</h3>"),
            C("Paragraph", "Typography", "<p>This is a paragraph of text. You can use it to describe anything.</p>"),
            C("Lead", "Typography", "<p class=\"lead\">This is a lead paragraph. It stands out from regular paragraphs.</p>"),
            C("Blockquote", "Typography",
                "<blockquote class=\"blockquote\">" +
                "<p>A well-known quote, contained in a blockquote element.</p>" +
                "<footer class=\"blockquote-footer\">Someone famous</footer>" +
                "</blockquote>"),

            // ── Forms ────────────────────────────────────────────────────────────────
            C("Input", "Forms",
                "<div class=\"mb-3\">" +
                "<label for=\"exampleInput\" class=\"form-label\">Email address</label>" +
                "<input type=\"email\" class=\"form-control\" id=\"exampleInput\" placeholder=\"name@example.com\">" +
                "</div>"),
            C("Textarea", "Forms",
                "<div class=\"mb-3\">" +
                "<label for=\"exampleTextarea\" class=\"form-label\">Example textarea</label>" +
                "<textarea class=\"form-control\" id=\"exampleTextarea\" rows=\"3\"></textarea>" +
                "</div>"),
            C("Select", "Forms",
                "<div class=\"mb-3\">" +
                "<label for=\"exampleSelect\" class=\"form-label\">Example select</label>" +
                "<select class=\"form-select\" id=\"exampleSelect\">" +
                "<option>Option 1</option><option>Option 2</option><option>Option 3</option>" +
                "</select></div>"),
            C("Checkbox", "Forms",
                "<div class=\"form-check\">" +
                "<input class=\"form-check-input\" type=\"checkbox\" id=\"flexCheck\">" +
                "<label class=\"form-check-label\" for=\"flexCheck\">Default checkbox</label>" +
                "</div>"),
        };

        await context.Components.AddRangeAsync(components);
        await context.SaveChangesAsync();
    }

    private static Component C(string type, string category, string content) => new()
    {
        Type = type,
        Category = category,
        Content = content
    };
}

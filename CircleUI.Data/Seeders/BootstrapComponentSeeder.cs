using CircleUI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CircleUI.Data.Seeders;

public static class BootstrapComponentSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        var categoriesToSeed = new[] { "Carousel" };
        var existingCategories = await context.Components
            .Where(c => c.IsTemplate)
            .Select(c => c.Category)
            .Distinct()
            .ToListAsync();

        var missingCategories = categoriesToSeed.Except(existingCategories).ToList();
        
        // If "Alerts" is already there and we aren't missing any specific new categories, we skip.
        if (existingCategories.Contains("Alerts") && !missingCategories.Any())
            return;

        var components = new List<Component>();
        
        if (!existingCategories.Contains("Carousel"))
        {
            components.AddRange(new[] {
                C("Carousel", "Carousel",
                    "<div id=\"carouselExample\" class=\"carousel slide\">" +
                    "<div class=\"carousel-inner\">" +
                    "<div class=\"carousel-item active\">" +
                    "<img src=\"https://placehold.co/800x400?text=Slide+1\" class=\"d-block w-100\" alt=\"Slide 1\">" +
                    "</div>" +
                    "<div class=\"carousel-item\">" +
                    "<img src=\"https://placehold.co/800x400?text=Slide+2\" class=\"d-block w-100\" alt=\"Slide 2\">" +
                    "</div>" +
                    "<div class=\"carousel-item\">" +
                    "<img src=\"https://placehold.co/800x400?text=Slide+3\" class=\"d-block w-100\" alt=\"Slide 3\">" +
                    "</div>" +
                    "</div>" +
                    "<button class=\"carousel-control-prev\" type=\"button\" data-bs-target=\"#carouselExample\" data-bs-slide=\"prev\">" +
                    "<span class=\"carousel-control-prev-icon\" aria-hidden=\"true\"></span>" +
                    "<span class=\"visually-hidden\">Previous</span>" +
                    "</button>" +
                    "<button class=\"carousel-control-next\" type=\"button\" data-bs-target=\"#carouselExample\" data-bs-slide=\"next\">" +
                    "<span class=\"carousel-control-next-icon\" aria-hidden=\"true\"></span>" +
                    "<span class=\"visually-hidden\">Next</span>" +
                    "</button>" +
                    "</div>"),
                C("Carousel Indicators", "Carousel",
                    "<div id=\"carouselExampleIndicators\" class=\"carousel slide\">" +
                    "<div class=\"carousel-indicators\">" +
                    "<button type=\"button\" data-bs-target=\"#carouselExampleIndicators\" data-bs-slide-to=\"0\" class=\"active\" aria-current=\"true\" aria-label=\"Slide 1\"></button>" +
                    "<button type=\"button\" data-bs-target=\"#carouselExampleIndicators\" data-bs-slide-to=\"1\" aria-label=\"Slide 2\"></button>" +
                    "<button type=\"button\" data-bs-target=\"#carouselExampleIndicators\" data-bs-slide-to=\"2\" aria-label=\"Slide 3\"></button>" +
                    "</div>" +
                    "<div class=\"carousel-inner\">" +
                    "<div class=\"carousel-item active\">" +
                    "<img src=\"https://placehold.co/800x400?text=Slide+1\" class=\"d-block w-100\" alt=\"Slide 1\">" +
                    "</div>" +
                    "<div class=\"carousel-item\">" +
                    "<img src=\"https://placehold.co/800x400?text=Slide+2\" class=\"d-block w-100\" alt=\"Slide 2\">" +
                    "</div>" +
                    "<div class=\"carousel-item\">" +
                    "<img src=\"https://placehold.co/800x400?text=Slide+3\" class=\"d-block w-100\" alt=\"Slide 3\">" +
                    "</div>" +
                    "</div>" +
                    "<button class=\"carousel-control-prev\" type=\"button\" data-bs-target=\"#carouselExampleIndicators\" data-bs-slide=\"prev\">" +
                    "<span class=\"carousel-control-prev-icon\" aria-hidden=\"true\"></span>" +
                    "<span class=\"visually-hidden\">Previous</span>" +
                    "</button>" +
                    "<button class=\"carousel-control-next\" type=\"button\" data-bs-target=\"#carouselExampleIndicators\" data-bs-slide=\"next\">" +
                    "<span class=\"carousel-control-next-icon\" aria-hidden=\"true\"></span>" +
                    "<span class=\"visually-hidden\">Next</span>" +
                    "</button>" +
                    "</div>")
            });
        }

        if (!existingCategories.Contains("Alerts"))
        {
            components.AddRange(new[] {
                // ── Alerts ──────────────────────────────────────────────────────────────
                C("Alert", "Alerts", "<div class=\"alert alert-primary\" role=\"alert\">A simple primary alert.</div>"),
                C("Alert", "Alerts", "<div class=\"alert alert-success\" role=\"alert\">A simple success alert.</div>"),
                C("Alert", "Alerts", "<div class=\"alert alert-warning\" role=\"alert\">A simple warning alert.</div>"),
                C("Alert", "Alerts", "<div class=\"alert alert-danger\" role=\"alert\">A simple danger alert.</div>"),
                C("Alert", "Alerts", "<div class=\"alert alert-info\" role=\"alert\">A simple info alert.</div>"),
            });
        }

        if (!existingCategories.Contains("Badges"))
        {
            components.AddRange(new[] {
                // ── Badges ──────────────────────────────────────────────────────────────
                C("Badge", "Badges", "<span class=\"badge text-bg-primary\">Primary</span>"),
                C("Badge", "Badges", "<span class=\"badge text-bg-success\">Success</span>"),
                C("Badge", "Badges", "<span class=\"badge text-bg-danger\">Danger</span>"),
                C("Badge", "Badges", "<span class=\"badge rounded-pill text-bg-secondary\">Pill</span>"),
            });
        }

        if (!existingCategories.Contains("Breadcrumb"))
        {
            components.AddRange(new[] {
                // ── Breadcrumb ───────────────────────────────────────────────────────────
                C("Breadcrumb", "Breadcrumb",
                    "<nav aria-label=\"breadcrumb\">" +
                    "<ol class=\"breadcrumb\">" +
                    "<li class=\"breadcrumb-item\"><a href=\"#\">Home</a></li>" +
                    "<li class=\"breadcrumb-item\"><a href=\"#\">Library</a></li>" +
                    "<li class=\"breadcrumb-item active\" aria-current=\"page\">Data</li>" +
                    "</ol></nav>"),
            });
        }

        if (!existingCategories.Contains("Buttons"))
        {
            components.AddRange(new[] {
                // ── Buttons ─────────────────────────────────────────────────────────────
                C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-primary\">Primary</button>"),
                C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-secondary\">Secondary</button>"),
                C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-success\">Success</button>"),
                C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-danger\">Danger</button>"),
                C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-outline-primary\">Outline Primary</button>"),
                C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-lg btn-primary\">Large Button</button>"),
                C("Button", "Buttons", "<button type=\"button\" class=\"btn btn-sm btn-primary\">Small Button</button>"),
            });
        }

        if (!existingCategories.Contains("Cards"))
        {
            components.AddRange(new[] {
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
            });
        }

        if (!existingCategories.Contains("List Groups"))
        {
            components.AddRange(new[] {
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
            });
        }

        if (!existingCategories.Contains("Navs"))
        {
            components.AddRange(new[] {
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
            });
        }

        if (!existingCategories.Contains("Navbar"))
        {
            components.AddRange(new[] {
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
            });
        }

        if (!existingCategories.Contains("Progress"))
        {
            components.AddRange(new[] {
                // ── Progress ─────────────────────────────────────────────────────────────
                C("Progress", "Progress",
                    "<div class=\"progress\" role=\"progressbar\" aria-valuenow=\"50\" aria-valuemin=\"0\" aria-valuemax=\"100\">" +
                    "<div class=\"progress-bar\" style=\"width:50%\">50%</div></div>"),
                C("Progress", "Progress",
                    "<div class=\"progress\" role=\"progressbar\" aria-valuenow=\"75\" aria-valuemin=\"0\" aria-valuemax=\"100\">" +
                    "<div class=\"progress-bar bg-success\" style=\"width:75%\">75%</div></div>"),
            });
        }

        if (!existingCategories.Contains("Spinners"))
        {
            components.AddRange(new[] {
                // ── Spinners ─────────────────────────────────────────────────────────────
                C("Spinner", "Spinners", "<div class=\"spinner-border\" role=\"status\"><span class=\"visually-hidden\">Loading...</span></div>"),
                C("Spinner", "Spinners", "<div class=\"spinner-border text-primary\" role=\"status\"><span class=\"visually-hidden\">Loading...</span></div>"),
                C("Spinner", "Spinners", "<div class=\"spinner-grow\" role=\"status\"><span class=\"visually-hidden\">Loading...</span></div>"),
            });
        }

        if (!existingCategories.Contains("Tables"))
        {
            components.AddRange(new[] {
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
            });
        }

        if (!existingCategories.Contains("Typography"))
        {
            components.AddRange(new[] {
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
            });
        }

        if (!existingCategories.Contains("Forms"))
        {
            components.AddRange(new[] {
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
            });
        }

        await context.Components.AddRangeAsync(components);
        await context.SaveChangesAsync();
    }

    private static Component C(string type, string category, string content) => new()
    {
        Type = type,
        Category = category,
        Content = content,
        IsTemplate = true
    };
}

using CircleUI.Core.DTOs;
using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.Data.Models;
using CircleUI.Tests.Helpers;
using NUnit.Framework;

namespace CircleUI.Tests.Services;

[TestFixture]
public class PageServiceTests
{
    private ApplicationDbContext _context = null!;
    private PageService _service = null!;
    private Guid _projectId;

    [SetUp]
    public async Task SetUp()
    {
        _context = TestDbContextFactory.Create($"PageTests_{Guid.NewGuid()}");
        _service = new PageService(_context);

        var project = new WebsiteProject { Name = "Test Project", UserId = "user1" };
        _context.WebsiteProjects.Add(project);
        await _context.SaveChangesAsync();
        _projectId = project.Id;
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    // --- GetAll ---

    [Test]
    public async Task GetAll_ReturnsEmpty_WhenNoPages()
    {
        var result = await _service.GetAll();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAll_ReturnsAllPages()
    {
        _context.Pages.AddRange(
            new Page { Title = "Home", Path = "/", ProjectId = _projectId },
            new Page { Title = "About", Path = "/about", ProjectId = _projectId }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAll_MapsFieldsCorrectly()
    {
        _context.Pages.Add(new Page
        {
            Title = "My Page",
            Path = "/my-page",
            MetaDescription = "A description",
            MetaKeywords = "keyword1, keyword2",
            ProjectId = _projectId
        });
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result[0].Title, Is.EqualTo("My Page"));
        Assert.That(result[0].Path, Is.EqualTo("/my-page"));
        Assert.That(result[0].MetaDescription, Is.EqualTo("A description"));
        Assert.That(result[0].MetaKeywords, Is.EqualTo("keyword1, keyword2"));
        Assert.That(result[0].ProjectId, Is.EqualTo(_projectId));
    }

    // --- Create ---

    [Test]
    public async Task Create_ReturnsCreatedPage()
    {
        var input = new PageDTO
        {
            Title = "New Page",
            Path = "/new",
            ProjectId = _projectId
        };

        var result = await _service.Create(input);

        Assert.That(result.Id, Is.Not.Null);
        Assert.That(result.Title, Is.EqualTo("New Page"));
        Assert.That(result.Path, Is.EqualTo("/new"));
    }

    [Test]
    public async Task Create_PersistsPageToDatabase()
    {
        var input = new PageDTO { Title = "Page", Path = "/page", ProjectId = _projectId };

        await _service.Create(input);

        Assert.That(_context.Pages.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task Create_AssignsOrderBasedOnExistingPages()
    {
        _context.Pages.Add(new Page { Title = "First", Path = "/first", ProjectId = _projectId, Order = 0 });
        await _context.SaveChangesAsync();

        var input = new PageDTO { Title = "Second", Path = "/second", ProjectId = _projectId };
        var result = await _service.Create(input);

        var stored = _context.Pages.Single(p => p.Id == result.Id);
        Assert.That(stored.Order, Is.EqualTo(1));
    }

    [Test]
    public async Task Create_FirstPage_HasOrderZero()
    {
        var input = new PageDTO { Title = "First", Path = "/first", ProjectId = _projectId };
        var result = await _service.Create(input);

        var stored = _context.Pages.Single(p => p.Id == result.Id);
        Assert.That(stored.Order, Is.EqualTo(0));
    }

    [Test]
    public async Task Create_MultiplePages_EachGetUniqueId()
    {
        var p1 = await _service.Create(new PageDTO { Title = "Page A", Path = "/a", ProjectId = _projectId });
        var p2 = await _service.Create(new PageDTO { Title = "Page B", Path = "/b", ProjectId = _projectId });

        Assert.That(p1.Id, Is.Not.EqualTo(p2.Id));
    }

    // --- GetById ---

    [Test]
    public async Task GetById_ReturnsCorrectPage()
    {
        var created = await _service.Create(new PageDTO { Title = "Find Me", Path = "/find", ProjectId = _projectId });

        var result = await _service.GetById(created.Id.ToString()!);

        Assert.That(result.Id, Is.EqualTo(created.Id));
        Assert.That(result.Title, Is.EqualTo("Find Me"));
    }

    [Test]
    public async Task GetById_MapsAllFields()
    {
        var created = await _service.Create(new PageDTO
        {
            Title = "Full Page",
            Path = "/full",
            MetaDescription = "Meta",
            MetaKeywords = "key",
            ProjectId = _projectId
        });

        var result = await _service.GetById(created.Id.ToString()!);

        Assert.That(result.MetaDescription, Is.EqualTo("Meta"));
        Assert.That(result.MetaKeywords, Is.EqualTo("key"));
        Assert.That(result.ProjectId, Is.EqualTo(_projectId));
    }

    // --- Update ---

    [Test]
    public async Task Update_ChangesTitle()
    {
        var created = await _service.Create(new PageDTO { Title = "Old", Path = "/old", ProjectId = _projectId });
        created.Title = "New";

        var result = await _service.Update(created);

        Assert.That(result.Title, Is.EqualTo("New"));
    }

    [Test]
    public async Task Update_ChangesPath()
    {
        var created = await _service.Create(new PageDTO { Title = "Page", Path = "/old-path", ProjectId = _projectId });
        created.Path = "/new-path";

        var result = await _service.Update(created);

        Assert.That(result.Path, Is.EqualTo("/new-path"));
    }

    [Test]
    public async Task Update_PersistsChangesToDatabase()
    {
        var created = await _service.Create(new PageDTO { Title = "Original", Path = "/orig", ProjectId = _projectId });
        created.Title = "Changed";

        await _service.Update(created);

        var stored = _context.Pages.Single(p => p.Id == created.Id);
        Assert.That(stored.Title, Is.EqualTo("Changed"));
    }

    [Test]
    public async Task Update_ChangesMetaFields()
    {
        var created = await _service.Create(new PageDTO { Title = "Meta Page", Path = "/meta", ProjectId = _projectId });
        created.MetaDescription = "Updated desc";
        created.MetaKeywords = "updated, keywords";

        var result = await _service.Update(created);

        Assert.That(result.MetaDescription, Is.EqualTo("Updated desc"));
        Assert.That(result.MetaKeywords, Is.EqualTo("updated, keywords"));
    }

    // --- Delete ---

    [Test]
    public async Task Delete_RemovesPageFromDatabase()
    {
        var created = await _service.Create(new PageDTO { Title = "Delete Me", Path = "/del", ProjectId = _projectId });

        await _service.Delete(created.Id.ToString()!);

        Assert.That(_context.Pages.Any(p => p.Id == created.Id), Is.False);
    }

    [Test]
    public async Task Delete_LeavesOtherPagesIntact()
    {
        var p1 = await _service.Create(new PageDTO { Title = "Keep", Path = "/keep", ProjectId = _projectId });
        var p2 = await _service.Create(new PageDTO { Title = "Remove", Path = "/remove", ProjectId = _projectId });

        await _service.Delete(p2.Id.ToString()!);

        Assert.That(_context.Pages.Count(), Is.EqualTo(1));
        Assert.That(_context.Pages.Single().Id, Is.EqualTo(p1.Id));
    }
}

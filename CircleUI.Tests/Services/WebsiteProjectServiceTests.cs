using CircleUI.Core.DTOs;
using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.Data.Models;
using CircleUI.Tests.Helpers;
using NUnit.Framework;

namespace CircleUI.Tests.Services;

[TestFixture]
public class WebsiteProjectServiceTests
{
    private ApplicationDbContext _context = null!;
    private WebsiteProjectService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _context = TestDbContextFactory.Create($"WebsiteProjectTests_{Guid.NewGuid()}");
        _service = new WebsiteProjectService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    // --- GetAll ---

    [Test]
    public async Task GetAll_ReturnsEmpty_WhenNoProjects()
    {
        var result = await _service.GetAll();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAll_ReturnsAllProjects()
    {
        _context.WebsiteProjects.AddRange(
            new WebsiteProject { Name = "Project A", UserId = "user1" },
            new WebsiteProject { Name = "Project B", UserId = "user2" }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAll_MapsFieldsCorrectly()
    {
        var project = new WebsiteProject
        {
            Name = "My Site",
            Description = "Desc",
            Domain = "mysite.com",
            IsPublished = true,
            UserId = "user1",
            BackgroundColor = "#fff",
            PrimaryTextColor = "#000",
            SecondaryTextColor = "#aaa",
            ButtonColor = "#00f",
            ButtonTextColor = "#fff"
        };
        _context.WebsiteProjects.Add(project);
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result[0].Name, Is.EqualTo("My Site"));
        Assert.That(result[0].Description, Is.EqualTo("Desc"));
        Assert.That(result[0].Domain, Is.EqualTo("mysite.com"));
        Assert.That(result[0].IsPublished, Is.True);
        Assert.That(result[0].UserId, Is.EqualTo("user1"));
        Assert.That(result[0].BackgroundColor, Is.EqualTo("#fff"));
    }

    // --- GetByUserId ---

    [Test]
    public async Task GetByUserId_ReturnsOnlyUserProjects()
    {
        _context.WebsiteProjects.AddRange(
            new WebsiteProject { Name = "UserA Project", UserId = "userA" },
            new WebsiteProject { Name = "UserB Project", UserId = "userB" }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetByUserId("userA");

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("UserA Project"));
    }

    [Test]
    public async Task GetByUserId_ReturnsEmpty_WhenUserHasNoProjects()
    {
        var result = await _service.GetByUserId("nonexistent");

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetByUserId_ReturnsMultipleProjects_ForSameUser()
    {
        _context.WebsiteProjects.AddRange(
            new WebsiteProject { Name = "Site 1", UserId = "user1" },
            new WebsiteProject { Name = "Site 2", UserId = "user1" },
            new WebsiteProject { Name = "Site 3", UserId = "user2" }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetByUserId("user1");

        Assert.That(result.Count, Is.EqualTo(2));
    }

    // --- Create ---

    [Test]
    public async Task Create_ReturnsCreatedProject()
    {
        var input = new WebsiteProjectDTO
        {
            Name = "New Project",
            Description = "A new site",
            Domain = "new.com",
            IsPublished = false,
            UserId = "user1",
            BackgroundColor = "#fff",
            PrimaryTextColor = "#000",
            SecondaryTextColor = "#888",
            ButtonColor = "#00f",
            ButtonTextColor = "#fff"
        };

        var result = await _service.Create(input);

        Assert.That(result.Id, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("New Project"));
        Assert.That(result.UserId, Is.EqualTo("user1"));
    }

    [Test]
    public async Task Create_PersistsProjectToDatabase()
    {
        var input = new WebsiteProjectDTO
        {
            Name = "Persisted Project",
            UserId = "user1"
        };

        await _service.Create(input);

        var count = _context.WebsiteProjects.Count();
        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public async Task Create_CreatesHeaderAndFooterSections()
    {
        var input = new WebsiteProjectDTO { Name = "Test", UserId = "user1" };

        var result = await _service.Create(input);

        var project = _context.WebsiteProjects.Single(p => p.Id == result.Id);
        Assert.That(project.HeaderSectionId, Is.Not.Null);
        Assert.That(project.FooterSectionId, Is.Not.Null);
    }

    // --- GetById ---

    [Test]
    public async Task GetById_ReturnsCorrectProject()
    {
        var input = new WebsiteProjectDTO { Name = "My Project", UserId = "user1" };
        var created = await _service.Create(input);

        var result = await _service.GetById(created.Id.ToString()!);

        Assert.That(result.Id, Is.EqualTo(created.Id));
        Assert.That(result.Name, Is.EqualTo("My Project"));
    }

    [Test]
    public async Task GetById_IncludesHeaderAndFooterSection()
    {
        var input = new WebsiteProjectDTO { Name = "With Sections", UserId = "user1" };
        var created = await _service.Create(input);

        var result = await _service.GetById(created.Id.ToString()!);

        Assert.That(result.HeaderSection, Is.Not.Null);
        Assert.That(result.FooterSection, Is.Not.Null);
    }

    [Test]
    public async Task GetById_ReturnsEmptyPagesList_WhenNoPages()
    {
        var input = new WebsiteProjectDTO { Name = "No Pages", UserId = "user1" };
        var created = await _service.Create(input);

        var result = await _service.GetById(created.Id.ToString()!);

        Assert.That(result.Pages, Is.Empty);
    }

    // --- Update ---

    [Test]
    public async Task Update_ChangesProjectName()
    {
        var created = await _service.Create(new WebsiteProjectDTO { Name = "Old Name", UserId = "user1" });

        created.Name = "New Name";
        var result = await _service.Update(created);

        Assert.That(result.Name, Is.EqualTo("New Name"));
    }

    [Test]
    public async Task Update_ChangesColors()
    {
        var created = await _service.Create(new WebsiteProjectDTO { Name = "Colorful", UserId = "user1", BackgroundColor = "#fff" });

        created.BackgroundColor = "#000";
        var result = await _service.Update(created);

        Assert.That(result.BackgroundColor, Is.EqualTo("#000"));
    }

    [Test]
    public async Task Update_PersistsChangesToDatabase()
    {
        var created = await _service.Create(new WebsiteProjectDTO { Name = "Original", UserId = "user1" });
        created.Name = "Updated";

        await _service.Update(created);

        var stored = _context.WebsiteProjects.Single(p => p.Id == created.Id);
        Assert.That(stored.Name, Is.EqualTo("Updated"));
    }

    [Test]
    public async Task Update_ChangesIsPublished()
    {
        var created = await _service.Create(new WebsiteProjectDTO { Name = "Draft", UserId = "user1", IsPublished = false });

        created.IsPublished = true;
        var result = await _service.Update(created);

        Assert.That(result.IsPublished, Is.True);
    }

    // --- Delete ---

    [Test]
    public async Task Delete_RemovesProjectFromDatabase()
    {
        var created = await _service.Create(new WebsiteProjectDTO { Name = "ToDelete", UserId = "user1" });

        await _service.Delete(created.Id.ToString()!);

        var exists = _context.WebsiteProjects.Any(p => p.Id == created.Id);
        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task Delete_LeavesOtherProjectsIntact()
    {
        var p1 = await _service.Create(new WebsiteProjectDTO { Name = "Keep Me", UserId = "user1" });
        var p2 = await _service.Create(new WebsiteProjectDTO { Name = "Delete Me", UserId = "user1" });

        await _service.Delete(p2.Id.ToString()!);

        var remaining = _context.WebsiteProjects.ToList();
        Assert.That(remaining.Count, Is.EqualTo(1));
        Assert.That(remaining[0].Id, Is.EqualTo(p1.Id));
    }

    // --- EnsureHeaderFooter ---

    [Test]
    public async Task EnsureHeaderFooter_CreatesHeaderAndFooter_WhenMissing()
    {
        var project = new WebsiteProject { Name = "No Header", UserId = "user1" };
        _context.WebsiteProjects.Add(project);
        await _context.SaveChangesAsync();

        var result = await _service.EnsureHeaderFooter(project.Id.ToString());

        var stored = _context.WebsiteProjects.Single(p => p.Id == project.Id);
        Assert.That(stored.HeaderSectionId, Is.Not.Null);
        Assert.That(stored.FooterSectionId, Is.Not.Null);
    }

    [Test]
    public async Task EnsureHeaderFooter_DoesNotDuplicate_WhenAlreadyExists()
    {
        var header = new Section { Name = "Global Header" };
        var footer = new Section { Name = "Global Footer" };
        _context.Sections.AddRange(header, footer);
        await _context.SaveChangesAsync();

        var project = new WebsiteProject
        {
            Name = "Has Header",
            UserId = "user1",
            HeaderSectionId = header.Id,
            FooterSectionId = footer.Id
        };
        _context.WebsiteProjects.Add(project);
        await _context.SaveChangesAsync();

        var sectionCountBefore = _context.Sections.Count();
        await _service.EnsureHeaderFooter(project.Id.ToString());
        var sectionCountAfter = _context.Sections.Count();

        Assert.That(sectionCountAfter, Is.EqualTo(sectionCountBefore));
    }

    [Test]
    public async Task EnsureHeaderFooter_ThrowsException_WhenProjectNotFound()
    {
        var fakeId = Guid.NewGuid().ToString();

        Assert.ThrowsAsync<Exception>(() => _service.EnsureHeaderFooter(fakeId));
    }

    // --- Duplicate ---

    [Test]
    public async Task Duplicate_CreatesNewProjectWithCopySuffix()
    {
        var original = await _service.Create(new WebsiteProjectDTO { Name = "Original", UserId = "user1" });

        var copy = await _service.Duplicate(original.Id.ToString()!, "user2");

        Assert.That(copy.Name, Is.EqualTo("Original (Copy)"));
    }

    [Test]
    public async Task Duplicate_AssignsNewOwner()
    {
        var original = await _service.Create(new WebsiteProjectDTO { Name = "Project", UserId = "user1" });

        var copy = await _service.Duplicate(original.Id.ToString()!, "user2");

        Assert.That(copy.UserId, Is.EqualTo("user2"));
    }

    [Test]
    public async Task Duplicate_SetsCopyAsUnpublished()
    {
        var created = await _service.Create(new WebsiteProjectDTO { Name = "Published", UserId = "user1", IsPublished = true });
        var stored = _context.WebsiteProjects.Single(p => p.Id == created.Id);
        stored.IsPublished = true;
        await _context.SaveChangesAsync();

        var copy = await _service.Duplicate(created.Id.ToString()!, "user2");

        Assert.That(copy.IsPublished, Is.False);
    }

    [Test]
    public async Task Duplicate_ThrowsException_WhenOriginalNotFound()
    {
        Assert.ThrowsAsync<Exception>(() => _service.Duplicate(Guid.NewGuid().ToString(), "user1"));
    }

    [Test]
    public async Task Duplicate_CopiesColors()
    {
        var created = await _service.Create(new WebsiteProjectDTO
        {
            Name = "Colorful",
            UserId = "user1",
            BackgroundColor = "#123456",
            PrimaryTextColor = "#abcdef"
        });

        var copy = await _service.Duplicate(created.Id.ToString()!, "user2");

        Assert.That(copy.BackgroundColor, Is.EqualTo("#123456"));
        Assert.That(copy.PrimaryTextColor, Is.EqualTo("#abcdef"));
    }
}

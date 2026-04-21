using CircleUI.Core.DTOs;
using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.Data.Models;
using CircleUI.Tests.Helpers;
using NUnit.Framework;

namespace CircleUI.Tests.Services;

[TestFixture]
public class PublishedVersionServiceTests
{
    private ApplicationDbContext _context = null!;
    private PublishedVersionService _service = null!;
    private Guid _projectId;

    [SetUp]
    public async Task SetUp()
    {
        _context = TestDbContextFactory.Create($"PublishedVersionTests_{Guid.NewGuid()}");
        _service = new PublishedVersionService(_context);

        var project = new WebsiteProject { Name = "Test", UserId = "user1" };
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
    public async Task GetAll_ReturnsEmpty_WhenNoVersions()
    {
        var result = await _service.GetAll();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAll_ReturnsAllVersions()
    {
        _context.PublishedVersions.AddRange(
            new PublishedVersion { PublishedAt = DateTime.UtcNow, VersionHash = "hash1", ProjectId = _projectId },
            new PublishedVersion { PublishedAt = DateTime.UtcNow, VersionHash = "hash2", ProjectId = _projectId }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAll_MapsFieldsCorrectly()
    {
        var publishedAt = new DateTime(2025, 1, 15, 10, 0, 0, DateTimeKind.Utc);
        _context.PublishedVersions.Add(new PublishedVersion
        {
            PublishedAt = publishedAt,
            VersionHash = "abc123",
            ProjectId = _projectId
        });
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result[0].VersionHash, Is.EqualTo("abc123"));
        Assert.That(result[0].PublishedAt, Is.EqualTo(publishedAt));
        Assert.That(result[0].ProjectId, Is.EqualTo(_projectId));
    }

    // --- Create ---

    [Test]
    public async Task Create_ReturnsCreatedVersion()
    {
        var input = new PublishedVersionDTO
        {
            PublishedAt = DateTime.UtcNow,
            VersionHash = "newHash",
            ProjectId = _projectId
        };

        var result = await _service.Create(input);

        Assert.That(result.Id, Is.Not.Null);
        Assert.That(result.VersionHash, Is.EqualTo("newHash"));
        Assert.That(result.ProjectId, Is.EqualTo(_projectId));
    }

    [Test]
    public async Task Create_PersistsVersionToDatabase()
    {
        var input = new PublishedVersionDTO { PublishedAt = DateTime.UtcNow, VersionHash = "h", ProjectId = _projectId };

        await _service.Create(input);

        Assert.That(_context.PublishedVersions.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task Create_SavesPublishedAt()
    {
        var time = new DateTime(2025, 6, 1, 12, 0, 0, DateTimeKind.Utc);
        var input = new PublishedVersionDTO { PublishedAt = time, VersionHash = "hash", ProjectId = _projectId };

        var result = await _service.Create(input);

        Assert.That(result.PublishedAt, Is.EqualTo(time));
    }

    // --- GetById ---

    [Test]
    public async Task GetById_ReturnsCorrectVersion()
    {
        var pv = new PublishedVersion { PublishedAt = DateTime.UtcNow, VersionHash = "findMe", ProjectId = _projectId };
        _context.PublishedVersions.Add(pv);
        await _context.SaveChangesAsync();

        var result = await _service.GetById(pv.Id.ToString());

        Assert.That(result.VersionHash, Is.EqualTo("findMe"));
        Assert.That(result.ProjectId, Is.EqualTo(_projectId));
    }

    // --- Update ---

    [Test]
    public async Task Update_ChangesVersionHash()
    {
        var pv = new PublishedVersion { PublishedAt = DateTime.UtcNow, VersionHash = "old", ProjectId = _projectId };
        _context.PublishedVersions.Add(pv);
        await _context.SaveChangesAsync();

        var result = await _service.Update(new PublishedVersionDTO
        {
            Id = pv.Id,
            PublishedAt = pv.PublishedAt,
            VersionHash = "new",
            ProjectId = _projectId
        });

        Assert.That(result.VersionHash, Is.EqualTo("new"));
    }

    [Test]
    public async Task Update_ChangesPublishedAt()
    {
        var original = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var updated = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        var pv = new PublishedVersion { PublishedAt = original, VersionHash = "h", ProjectId = _projectId };
        _context.PublishedVersions.Add(pv);
        await _context.SaveChangesAsync();

        var result = await _service.Update(new PublishedVersionDTO
        {
            Id = pv.Id,
            PublishedAt = updated,
            VersionHash = "h",
            ProjectId = _projectId
        });

        Assert.That(result.PublishedAt, Is.EqualTo(updated));
    }

    [Test]
    public async Task Update_PersistsChangesToDatabase()
    {
        var pv = new PublishedVersion { PublishedAt = DateTime.UtcNow, VersionHash = "before", ProjectId = _projectId };
        _context.PublishedVersions.Add(pv);
        await _context.SaveChangesAsync();

        await _service.Update(new PublishedVersionDTO { Id = pv.Id, PublishedAt = pv.PublishedAt, VersionHash = "after", ProjectId = _projectId });

        var stored = _context.PublishedVersions.Single(v => v.Id == pv.Id);
        Assert.That(stored.VersionHash, Is.EqualTo("after"));
    }

    // --- Delete ---

    [Test]
    public async Task Delete_RemovesVersionFromDatabase()
    {
        var pv = new PublishedVersion { PublishedAt = DateTime.UtcNow, VersionHash = "del", ProjectId = _projectId };
        _context.PublishedVersions.Add(pv);
        await _context.SaveChangesAsync();

        await _service.Delete(pv.Id.ToString());

        Assert.That(_context.PublishedVersions.Any(v => v.Id == pv.Id), Is.False);
    }

    [Test]
    public async Task Delete_LeavesOtherVersionsIntact()
    {
        var v1 = new PublishedVersion { PublishedAt = DateTime.UtcNow, VersionHash = "keep", ProjectId = _projectId };
        var v2 = new PublishedVersion { PublishedAt = DateTime.UtcNow, VersionHash = "gone", ProjectId = _projectId };
        _context.PublishedVersions.AddRange(v1, v2);
        await _context.SaveChangesAsync();

        await _service.Delete(v2.Id.ToString());

        Assert.That(_context.PublishedVersions.Count(), Is.EqualTo(1));
        Assert.That(_context.PublishedVersions.Single().Id, Is.EqualTo(v1.Id));
    }
}

using CircleUI.Core.DTOs;
using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.Data.Models;
using CircleUI.Tests.Helpers;
using NUnit.Framework;

namespace CircleUI.Tests.Services;

[TestFixture]
public class AssetServiceTests
{
    private ApplicationDbContext _context = null!;
    private AssetService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _context = TestDbContextFactory.Create($"AssetTests_{Guid.NewGuid()}");
        _service = new AssetService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    // --- GetAll ---

    [Test]
    public async Task GetAll_ReturnsEmpty_WhenNoAssets()
    {
        var result = await _service.GetAll();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAll_ReturnsAllAssets()
    {
        _context.Assets.AddRange(
            new Asset { FileName = "img1.png", MimeType = "image/png", SizeBytes = 100, StoragePath = "/uploads/1", UserId = "user1" },
            new Asset { FileName = "img2.jpg", MimeType = "image/jpeg", SizeBytes = 200, StoragePath = "/uploads/2", UserId = "user2" }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAll_MapsFieldsCorrectly()
    {
        _context.Assets.Add(new Asset
        {
            FileName = "photo.png",
            MimeType = "image/png",
            SizeBytes = 512,
            StoragePath = "/uploads/user1/photo.png",
            UserId = "user1"
        });
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result[0].FileName, Is.EqualTo("photo.png"));
        Assert.That(result[0].MimeType, Is.EqualTo("image/png"));
        Assert.That(result[0].SizeBytes, Is.EqualTo(512));
        Assert.That(result[0].StoragePath, Is.EqualTo("/uploads/user1/photo.png"));
        Assert.That(result[0].UserId, Is.EqualTo("user1"));
    }

    // --- GetByUserId ---

    [Test]
    public async Task GetByUserId_ReturnsOnlyUserAssets()
    {
        _context.Assets.AddRange(
            new Asset { FileName = "a.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/a", UserId = "userA" },
            new Asset { FileName = "b.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/b", UserId = "userB" }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetByUserId("userA");

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].FileName, Is.EqualTo("a.png"));
    }

    [Test]
    public async Task GetByUserId_ReturnsEmpty_WhenUserHasNoAssets()
    {
        var result = await _service.GetByUserId("nobody");

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetByUserId_ReturnsMultipleAssets_ForSameUser()
    {
        _context.Assets.AddRange(
            new Asset { FileName = "x.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/x", UserId = "u1" },
            new Asset { FileName = "y.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/y", UserId = "u1" },
            new Asset { FileName = "z.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/z", UserId = "u2" }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetByUserId("u1");

        Assert.That(result.Count, Is.EqualTo(2));
    }

    // --- Create ---

    [Test]
    public async Task Create_ReturnsCreatedAsset()
    {
        var input = new AssetDTO
        {
            FileName = "new.png",
            MimeType = "image/png",
            SizeBytes = 1024,
            StoragePath = "/uploads/new.png",
            UserId = "user1"
        };

        var result = await _service.Create(input);

        Assert.That(result.Id, Is.Not.Null);
        Assert.That(result.FileName, Is.EqualTo("new.png"));
        Assert.That(result.UserId, Is.EqualTo("user1"));
    }

    [Test]
    public async Task Create_PersistsAssetToDatabase()
    {
        var input = new AssetDTO { FileName = "f.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/f", UserId = "u" };

        await _service.Create(input);

        Assert.That(_context.Assets.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task Create_SavesSizeBytes()
    {
        var input = new AssetDTO { FileName = "big.png", MimeType = "image/png", SizeBytes = 999999, StoragePath = "/big", UserId = "u" };

        var result = await _service.Create(input);

        Assert.That(result.SizeBytes, Is.EqualTo(999999));
    }

    // --- GetById ---

    [Test]
    public async Task GetById_ReturnsCorrectAsset()
    {
        var asset = new Asset { FileName = "find.png", MimeType = "image/png", SizeBytes = 10, StoragePath = "/find", UserId = "u" };
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        var result = await _service.GetById(asset.Id.ToString());

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(asset.Id));
        Assert.That(result.FileName, Is.EqualTo("find.png"));
    }

    [Test]
    public async Task GetById_ReturnsNull_WhenAssetNotFound()
    {
        var result = await _service.GetById(Guid.NewGuid().ToString());

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetById_ReturnsNull_WhenIdIsInvalidGuid()
    {
        var result = await _service.GetById("not-a-guid");

        Assert.That(result, Is.Null);
    }

    // --- Update ---

    [Test]
    public async Task Update_ChangesFileName()
    {
        var asset = new Asset { FileName = "old.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/old", UserId = "u" };
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        var result = await _service.Update(new AssetDTO
        {
            Id = asset.Id,
            FileName = "new.png",
            MimeType = "image/png",
            SizeBytes = 1,
            StoragePath = "/old",
            UserId = "u"
        });

        Assert.That(result.FileName, Is.EqualTo("new.png"));
    }

    [Test]
    public async Task Update_ChangesStoragePath()
    {
        var asset = new Asset { FileName = "f.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/old", UserId = "u" };
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        var result = await _service.Update(new AssetDTO
        {
            Id = asset.Id,
            FileName = "f.png",
            MimeType = "image/png",
            SizeBytes = 1,
            StoragePath = "/new",
            UserId = "u"
        });

        Assert.That(result.StoragePath, Is.EqualTo("/new"));
    }

    [Test]
    public async Task Update_PersistsChangesToDatabase()
    {
        var asset = new Asset { FileName = "orig.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/", UserId = "u" };
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        await _service.Update(new AssetDTO { Id = asset.Id, FileName = "updated.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/", UserId = "u" });

        var stored = _context.Assets.Single(a => a.Id == asset.Id);
        Assert.That(stored.FileName, Is.EqualTo("updated.png"));
    }

    // --- Delete ---

    [Test]
    public async Task Delete_RemovesAssetFromDatabase()
    {
        var asset = new Asset { FileName = "del.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/", UserId = "u" };
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        await _service.Delete(asset.Id.ToString());

        Assert.That(_context.Assets.Any(a => a.Id == asset.Id), Is.False);
    }

    [Test]
    public async Task Delete_LeavesOtherAssetsIntact()
    {
        var a1 = new Asset { FileName = "keep.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/k", UserId = "u" };
        var a2 = new Asset { FileName = "gone.png", MimeType = "image/png", SizeBytes = 1, StoragePath = "/g", UserId = "u" };
        _context.Assets.AddRange(a1, a2);
        await _context.SaveChangesAsync();

        await _service.Delete(a2.Id.ToString());

        Assert.That(_context.Assets.Count(), Is.EqualTo(1));
        Assert.That(_context.Assets.Single().Id, Is.EqualTo(a1.Id));
    }
}

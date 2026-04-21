using CircleUI.Core.DTOs;
using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.Data.Models;
using CircleUI.Tests.Helpers;
using NUnit.Framework;

namespace CircleUI.Tests.Services;

[TestFixture]
public class UserServiceTests
{
    private ApplicationDbContext _context = null!;
    private UserService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _context = TestDbContextFactory.Create($"UserTests_{Guid.NewGuid()}");
        _service = new UserService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    // --- GetAll ---

    [Test]
    public async Task GetAll_ReturnsEmpty_WhenNoUsers()
    {
        var result = await _service.GetAll();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAll_ReturnsAllUsers()
    {
        _context.Users.AddRange(
            new User { DisplayName = "Alice", UserName = "alice", Email = "alice@example.com" },
            new User { DisplayName = "Bob", UserName = "bob", Email = "bob@example.com" }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAll_MapsFieldsCorrectly()
    {
        _context.Users.Add(new User
        {
            DisplayName = "Charlie",
            UserName = "charlie",
            Email = "charlie@example.com"
        });
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result[0].DisplayName, Is.EqualTo("Charlie"));
        Assert.That(result[0].UserName, Is.EqualTo("charlie"));
        Assert.That(result[0].Email, Is.EqualTo("charlie@example.com"));
        Assert.That(result[0].Id, Is.Not.Null);
    }

    // --- Create ---

    [Test]
    public async Task Create_ReturnsCreatedUser()
    {
        var input = new UserDTO
        {
            DisplayName = "Dave",
            UserName = "dave",
            Email = "dave@example.com"
        };

        var result = await _service.Create(input);

        Assert.That(result.Id, Is.Not.Null);
        Assert.That(result.DisplayName, Is.EqualTo("Dave"));
        Assert.That(result.Email, Is.EqualTo("dave@example.com"));
    }

    [Test]
    public async Task Create_PersistsUserToDatabase()
    {
        var input = new UserDTO { DisplayName = "Eve", UserName = "eve", Email = "eve@example.com" };

        await _service.Create(input);

        Assert.That(_context.Users.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task Create_SavesDisplayName()
    {
        var input = new UserDTO { DisplayName = "Frank User", UserName = "frank", Email = "frank@example.com" };

        var result = await _service.Create(input);

        Assert.That(result.DisplayName, Is.EqualTo("Frank User"));
    }

    // --- GetById ---

    [Test]
    public async Task GetById_ReturnsCorrectUser()
    {
        var user = new User { DisplayName = "Grace", UserName = "grace", Email = "grace@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _service.GetById(user.Id);

        Assert.That(result.Id, Is.EqualTo(user.Id));
        Assert.That(result.DisplayName, Is.EqualTo("Grace"));
    }

    [Test]
    public async Task GetById_MapsAllFields()
    {
        var user = new User { DisplayName = "Hank", UserName = "hank", Email = "hank@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _service.GetById(user.Id);

        Assert.That(result.UserName, Is.EqualTo("hank"));
        Assert.That(result.Email, Is.EqualTo("hank@example.com"));
    }

    // --- Update ---

    [Test]
    public async Task Update_ChangesDisplayName()
    {
        var user = new User { DisplayName = "Old Name", UserName = "user", Email = "user@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _service.Update(new UserDTO
        {
            Id = user.Id,
            DisplayName = "New Name",
            UserName = "user",
            Email = "user@example.com"
        });

        Assert.That(result.DisplayName, Is.EqualTo("New Name"));
    }

    [Test]
    public async Task Update_ChangesEmail()
    {
        var user = new User { DisplayName = "Iris", UserName = "iris", Email = "old@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _service.Update(new UserDTO
        {
            Id = user.Id,
            DisplayName = "Iris",
            UserName = "iris",
            Email = "new@example.com"
        });

        Assert.That(result.Email, Is.EqualTo("new@example.com"));
    }

    [Test]
    public async Task Update_PersistsChangesToDatabase()
    {
        var user = new User { DisplayName = "Jack", UserName = "jack", Email = "jack@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await _service.Update(new UserDTO { Id = user.Id, DisplayName = "Jack Updated", UserName = "jack", Email = "jack@example.com" });

        var stored = _context.Users.Single(u => u.Id == user.Id);
        Assert.That(stored.DisplayName, Is.EqualTo("Jack Updated"));
    }

    // --- Delete ---

    [Test]
    public async Task Delete_RemovesUserFromDatabase()
    {
        var user = new User { DisplayName = "Karen", UserName = "karen", Email = "karen@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await _service.Delete(user.Id);

        Assert.That(_context.Users.Any(u => u.Id == user.Id), Is.False);
    }

    [Test]
    public async Task Delete_LeavesOtherUsersIntact()
    {
        var u1 = new User { DisplayName = "Leo", UserName = "leo", Email = "leo@example.com" };
        var u2 = new User { DisplayName = "Mia", UserName = "mia", Email = "mia@example.com" };
        _context.Users.AddRange(u1, u2);
        await _context.SaveChangesAsync();

        await _service.Delete(u2.Id);

        Assert.That(_context.Users.Count(), Is.EqualTo(1));
        Assert.That(_context.Users.Single().Id, Is.EqualTo(u1.Id));
    }
}

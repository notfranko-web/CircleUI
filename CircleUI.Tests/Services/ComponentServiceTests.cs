using CircleUI.Core.DTOs;
using CircleUI.Core.Services;
using CircleUI.Data;
using CircleUI.Data.Models;
using CircleUI.Tests.Helpers;
using NUnit.Framework;

namespace CircleUI.Tests.Services;

[TestFixture]
public class ComponentServiceTests
{
    private ApplicationDbContext _context = null!;
    private ComponentService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _context = TestDbContextFactory.Create($"ComponentTests_{Guid.NewGuid()}");
        _service = new ComponentService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    // --- GetAll ---

    [Test]
    public async Task GetAll_ReturnsEmpty_WhenNoTemplateComponents()
    {
        var result = await _service.GetAll();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAll_ReturnsOnlyTemplateComponents()
    {
        _context.Components.AddRange(
            new Component { Type = "Button", IsTemplate = true, Content = "<btn>", Layout = "{}" },
            new Component { Type = "Card", IsTemplate = false, Content = "<card>", Layout = "{}" }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo("Button"));
    }

    [Test]
    public async Task GetAll_ReturnsMultipleTemplates()
    {
        _context.Components.AddRange(
            new Component { Type = "A", IsTemplate = true, Content = "", Layout = "{}" },
            new Component { Type = "B", IsTemplate = true, Content = "", Layout = "{}" },
            new Component { Type = "C", IsTemplate = false, Content = "", Layout = "{}" }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAll_MapsFieldsCorrectly()
    {
        _context.Components.Add(new Component
        {
            Type = "NavBar",
            Category = "Navigation",
            Content = "<nav>Test</nav>",
            Layout = "{\"desktop\":{\"x\":0}}",
            IsTemplate = true
        });
        await _context.SaveChangesAsync();

        var result = await _service.GetAll();

        Assert.That(result[0].Type, Is.EqualTo("NavBar"));
        Assert.That(result[0].Category, Is.EqualTo("Navigation"));
        Assert.That(result[0].Content, Is.EqualTo("<nav>Test</nav>"));
        Assert.That(result[0].Layout, Is.EqualTo("{\"desktop\":{\"x\":0}}"));
    }

    // --- Create ---

    [Test]
    public async Task Create_ReturnsCreatedComponent()
    {
        var input = new ComponentDTO
        {
            Type = "Hero",
            Content = "<div>Hero</div>",
            Layout = "{}"
        };

        var result = await _service.Create(input);

        Assert.That(result.Id, Is.Not.Null);
        Assert.That(result.Type, Is.EqualTo("Hero"));
        Assert.That(result.Content, Is.EqualTo("<div>Hero</div>"));
    }

    [Test]
    public async Task Create_PersistsComponentToDatabase()
    {
        var input = new ComponentDTO { Type = "Text", Content = "<p>Hi</p>", Layout = "{}" };

        await _service.Create(input);

        Assert.That(_context.Components.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task Create_SavesLayoutCorrectly()
    {
        var layout = "{\"desktop\":{\"x\":0,\"y\":0,\"w\":12}}";
        var input = new ComponentDTO { Type = "Grid", Content = "", Layout = layout };

        var result = await _service.Create(input);

        var stored = _context.Components.Single(c => c.Id == result.Id);
        Assert.That(stored.Layout, Is.EqualTo(layout));
    }

    // --- GetById ---

    [Test]
    public async Task GetById_ReturnsCorrectComponent()
    {
        var component = new Component { Type = "Alert", Content = "<div class='alert'>", Layout = "{}", IsTemplate = true };
        _context.Components.Add(component);
        await _context.SaveChangesAsync();

        var result = await _service.GetById(component.Id.ToString());

        Assert.That(result.Id, Is.EqualTo(component.Id));
        Assert.That(result.Type, Is.EqualTo("Alert"));
    }

    [Test]
    public async Task GetById_MapsContentAndLayout()
    {
        var layout = "{\"mobile\":{\"x\":0}}";
        var component = new Component { Type = "Footer", Content = "<footer>", Layout = layout, IsTemplate = false };
        _context.Components.Add(component);
        await _context.SaveChangesAsync();

        var result = await _service.GetById(component.Id.ToString());

        Assert.That(result.Content, Is.EqualTo("<footer>"));
        Assert.That(result.Layout, Is.EqualTo(layout));
    }

    // --- Update ---

    [Test]
    public async Task Update_ChangesType()
    {
        var component = new Component { Type = "Old", Content = "", Layout = "{}", IsTemplate = true };
        _context.Components.Add(component);
        await _context.SaveChangesAsync();

        var result = await _service.Update(new ComponentDTO { Id = component.Id, Type = "New", Content = "", Layout = "{}" });

        Assert.That(result.Type, Is.EqualTo("New"));
    }

    [Test]
    public async Task Update_ChangesContent()
    {
        var component = new Component { Type = "Btn", Content = "<old>", Layout = "{}", IsTemplate = true };
        _context.Components.Add(component);
        await _context.SaveChangesAsync();

        var result = await _service.Update(new ComponentDTO { Id = component.Id, Type = "Btn", Content = "<new>", Layout = "{}" });

        Assert.That(result.Content, Is.EqualTo("<new>"));
    }

    [Test]
    public async Task Update_PersistsChangesToDatabase()
    {
        var component = new Component { Type = "X", Content = "old", Layout = "{}", IsTemplate = false };
        _context.Components.Add(component);
        await _context.SaveChangesAsync();

        await _service.Update(new ComponentDTO { Id = component.Id, Type = "X", Content = "new", Layout = "{}" });

        var stored = _context.Components.Single(c => c.Id == component.Id);
        Assert.That(stored.Content, Is.EqualTo("new"));
    }

    [Test]
    public async Task Update_ChangesLayout()
    {
        var component = new Component { Type = "C", Content = "", Layout = "{}", IsTemplate = false };
        _context.Components.Add(component);
        await _context.SaveChangesAsync();

        var newLayout = "{\"desktop\":{\"w\":6}}";
        var result = await _service.Update(new ComponentDTO { Id = component.Id, Type = "C", Content = "", Layout = newLayout });

        Assert.That(result.Layout, Is.EqualTo(newLayout));
    }

    // --- Delete ---

    [Test]
    public async Task Delete_RemovesComponentFromDatabase()
    {
        var component = new Component { Type = "ToDelete", Content = "", Layout = "{}", IsTemplate = false };
        _context.Components.Add(component);
        await _context.SaveChangesAsync();

        await _service.Delete(component.Id.ToString());

        Assert.That(_context.Components.Any(c => c.Id == component.Id), Is.False);
    }

    [Test]
    public async Task Delete_LeavesOtherComponentsIntact()
    {
        var c1 = new Component { Type = "Keep", Content = "", Layout = "{}", IsTemplate = true };
        var c2 = new Component { Type = "Remove", Content = "", Layout = "{}", IsTemplate = false };
        _context.Components.AddRange(c1, c2);
        await _context.SaveChangesAsync();

        await _service.Delete(c2.Id.ToString());

        Assert.That(_context.Components.Count(), Is.EqualTo(1));
        Assert.That(_context.Components.Single().Id, Is.EqualTo(c1.Id));
    }
}

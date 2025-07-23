using Microsoft.AspNetCore.Mvc;
using VCardManager.Core;

namespace VCardManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VCardController : ControllerBase
{
    private static readonly List<VCard> _contacts = new();

    [HttpGet]
    public IActionResult GetAll() => Ok(_contacts);

    [HttpPost]
    public IActionResult Add([FromBody] VCard card)
    {
        _contacts.Add(card);
        return Ok(card);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var contact = _contacts.FirstOrDefault(x => x.Id == id);
        if (contact == null)
            return NotFound();

        _contacts.Remove(contact);
        return NoContent();
    }

    [HttpGet("export")]
    public IActionResult Export()
    {
        var content = string.Join("\n", _contacts.Select(c => c.ToVcf()));
        var bytes = System.Text.Encoding.UTF8.GetBytes(content);
        return File(bytes, "text/vcard", "contacts.vcf");
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Empty file");

        using var reader = new StreamReader(file.OpenReadStream());
        var fileContent = await reader.ReadToEndAsync();

        var cards = fileContent.Split("BEGIN:VCARD")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => VCard.FromVcf("BEGIN:VCARD" + x.Trim()))
            .ToList();

        _contacts.AddRange(cards);
        return Ok(cards);
    }
}

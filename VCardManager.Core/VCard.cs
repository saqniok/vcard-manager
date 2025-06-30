namespace VCardManager.Core;

public class VCard
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"BEGIN:VCARD\n" +
                $"FullName:{FullName}\n" +
                $"TEL:{PhoneNumber}\n" +
                $"EMAIL:{Email}\n" +
                $"ID:{Id}\n" +
                $"END:VCARD";
    }

    public static VCard FromVcf(string vcf)
    {
        var lines = vcf.Split('\n');
        var card = new VCard
        {
            Id = Guid.TryParse(lines.FirstOrDefault(x => x.StartsWith("ID:"))?.Substring(3).Trim(), out var guid) ? guid : Guid.NewGuid(),
            FullName = lines.FirstOrDefault(x => x.StartsWith("FullName:"))?.Substring(9).Trim() ?? string.Empty,
            PhoneNumber = lines.FirstOrDefault(x => x.StartsWith("TEL:"))?.Substring(4).Trim() ?? string.Empty,
            Email = lines.FirstOrDefault(x => x.StartsWith("EMAIL:"))?.Substring(6).Trim() ?? string.Empty
        };
        return card;
    }
}

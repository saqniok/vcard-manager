namespace VCardManager.Core;

public class VCard
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public override string ToString()
    {
        return  $"BEGIN:VCARD\n" +
                $"Full Name: {FullName}\n" +
                $"TEL: {PhoneNumber}\n" +
                $"EMAIL: {Email}\n" +
                $"END:VCARD";
    }

    public static VCard FromVcf(string vcf)
    {
        var lines = vcf.Split('\n');
        var card = new VCard
        {
            FullName = lines.FirstOrDefault(x => x.StartsWith("Full Name: "))?.Substring(10).Trim() ?? string.Empty,
            PhoneNumber = lines.FirstOrDefault(x => x.StartsWith("TEL: "))?.Substring(4).Trim() ?? string.Empty,
            Email = lines.FirstOrDefault(x => x.StartsWith("EMAIL:"))?.Substring(6).Trim() ?? string.Empty
        };
        return card;
    }
}

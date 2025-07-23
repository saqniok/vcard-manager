namespace VCardManager.Core;

public class VCard
{
    // Properties
    public Guid Id { get; set; } = Guid.NewGuid();
    /// <summary>
    ///     This is a declaration of a property with the name `Id` of type `Guid` (Globally Unique Identifier)
    ///     - Guid - is a data type used to create unique 128-bit numbers
    ///     - `Guid.NewGuid();` is the default property initializer. It assigns a unique Guid to a new VCard object when it is created
    /// </summary>

    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string ToVcf() => ToString();

    public override string ToString()                      //   `override` indicates that this method overrides the ToString() method 
    {                                                      //   from the object base class (from which all classes in C# implicitly inherit). 
        return $"BEGIN:VCARD\n" +                          //   The standard ToString() normally returns a type name. Here, however, 
                $"FullName:{FullName}\n" +                 //   it is overridden to return a formatted string representing a VCard in .vcf format
                $"TEL:{PhoneNumber}\n" +
                $"EMAIL:{Email}\n" +
                $"ID:{Id}\n" +
                $"END:VCARD";
    }

    public static VCard FromVcf(string vcf)
    /// <summary>
    ///     `static` means that this method belongs to the VCard class itself, 
    ///     not to a specific instance of a VCard object. You can call it directly using
    ///     the class name (e.g., VCard.FromVcf(...)) without having to create a VCard object. 
    ///     
    ///     This is useful for factory methods that create objects
    /// <summary>

    {
        var lines = vcf.Split('\n');            // This line `splits` the input vcf string into a string array by a newline character (\n)
                                                //  Each line in the lines array will correspond to one line from the VCard data

        var card = new VCard                    // This line creates a new instance of the VCard class and initializes its properties using the object initializer syntax
        {
            Id = Guid.TryParse(lines.FirstOrDefault(x => x.StartsWith("ID:"))?.Substring(3).Trim(), out var guid) ? guid : Guid.NewGuid(),
            FullName = lines.FirstOrDefault(x => x.StartsWith("FullName:"))?.Substring(9).Trim() ?? string.Empty,
            PhoneNumber = lines.FirstOrDefault(x => x.StartsWith("TEL:"))?.Substring(4).Trim() ?? string.Empty,
            Email = lines.FirstOrDefault(x => x.StartsWith("EMAIL:"))?.Substring(6).Trim() ?? string.Empty
        };
        return card;
    }
}
/*
    Id = Guid.TryParse(lines.FirstOrDefault(x => x.StartsWith("ID:"))?.Substring(3).Trim(), out var guid) ? guid : Guid.NewGuid()

    #   `Guid.TryParse`                     - Tries to parse a span of characters into a value

    #   `.FirstOrDefault(...)`              - is a LINQ extension method that searches for the first element in the collection that matches the given condition. 
                                            If no element is found, it returns the default value for the type (null for string)

    #   `x => x.StartsWith(“ID:”)`          - is a lambda expression that defines the search condition: the string must start with “ID:”

    #   `?.` (after FirstOrDefault(...))    - is a null-conditional operator. If the result of FirstOrDefault is not null (i.e., the string “ID:” is found),
                                            then the execution of the expression after ? continues. If null, the entire expression returns null 
                                            without causing a NullReferenceException

    #   `.Substring(n)`                     - if a string is found, this method extracts the substring starting from the `n` character (index n), i.e. skips “ID:”

    #   `.Trim()`                           - this method removes all whitespace characters (spaces, tabs, line breaks) from the beginning and end of the string

    #   `out var`                           - is an out parameter. If TryParse returns `true`, the successfully parsed Guid is stored in a new variable guid
                                            The unparsed `Guid` is returned not via `return`, but via the `out` parameter `variable` - in this case `guid`
                                            
    #   TO MME:                             - Why `var`? - idk, because we kind of know what .TryParse() will return - true, but if false, 
                                            then in the next code, we will create a new guid and assign it to guid.NewGuid();
                                            That's right? can we write `out Guid guid`? What's the difference?

    # `?? string.Empty`                     - `??` is a null-coalescing operator. If the result of the expression to the left of ?? is null 
                                            (i.e., the corresponding string was not found or Substring returned null), then string.Empty is assigned. 
                                            Otherwise, the extracted value is assigned
*/

## Code Review

### Needs More Tests ;-)
- VCard.FromVcf(string vcf), although implicitely covered I think
- Menu
- Facade

### Facade
- Try moving user interaction to seperate class(es)
- Chop `SelectCardByName` into smaller pieces

### Duplicate code :
```csharp
_console.WriteLine("Add name: ");
var name = _console.ReadLine();

_console.WriteLine("Add Phone Number: ");
var phone = _console.ReadLine();
```
### Some Trailing Dust
```csharp
public void ShowCard()
{

}
``` 
### You Could ...
- split CardService into CardService and CardRepository, the latter handling CRUD, i.e. storage

### Overall pretty solid implementation though 





## Code Review

### WaTCh YouR CaSInG :
* methodes => PascalCase. f.i. `CardProperty.parsedCards(...)`, `_cardService.getAllCards()`.

### Too Defensive 
* This is redundant, especially with CI : 
> `_fileStore = fileStore ?? throw new ArgumentNullException(nameof(fileStore));`

### Brittle Tests
* `CardServiceTest.DeleteCard_ShouldRemoveCardFromFile`
You are using implementation to Assert things =>
```csharp
// After Remove
_cardService.deleteCard(card2.Id);
var afterDeleteCard = _cardService.getAllCards().ToList(); // <= getAllCards
``` 
### Regex 
* *Simple* `Why ?: ` comment needed showing what is valid.

### You Could ...
- split CardService into CardService and CardRepository, the latter handling CRUD, i.e. storage





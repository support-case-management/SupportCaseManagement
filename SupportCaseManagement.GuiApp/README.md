# Support Case Management System (MVP)

Ett ärendehanteringssystem byggt med WinUI 3, C# och Domain-Driven Design (DDD).

## Gruppmedlemmar Arbetsfördelning (Grupp 1)
* **Vüsal** – Översiktsvy (Dashboard), Skapa ärende, README & Dokumentation.
* **Mia** – Ärendelista, Sök/Filtrering & Enhetstester, Kundlistan samt skapa kunder.
* **Silas** – Redigera ärenden, Detaljvy & Kommentarshantering.

---

## Kontrollista över uppfyllda krav
- [x] Skapa och hantera kunder (Privat- och Företagskunder).
- [x] Skapa och koppla ärenden till en specifik kund.
- [x] Sätta prioritet (Låg, Medium, Hög) och status (Nytt, Pågående, Löst) på ärenden.
- [x] Söka och filtrera i ärendelistan.
- [x] Redigera befintliga ärenden och kunduppgifter.
- [x] Kommentarshantering på ärenden (med den senaste kommentaren överst).
- [x] Strukturera koden enligt DDD (Domain-Driven Design) i 4 lager.
- [x] Persistera data lokalt i JSON-format.

## Applikationens Syfte
Applikationens syfte är att göra det enkelt för en användare att hantera supportärenden. Användaren kan skapa kunder (privat- eller företagskunder) och därefter skapa ärenden som kopplas till en specifik kund. För att enkelt kunna följa och prioritera arbetet har varje ärende en specifik status (Nytt, Pågående, Löst) och prioritet (Låg, Medium, Hög).

---

## Arkitektur & Projektstruktur (DDD)
Projektet är uppbyggt enligt principen för **Domain-Driven Design (DDD)** med fyra tydligt separerade lager för att underlätta testning, debuggning och vidareutveckling:

1. **`SupportCaseManagement.Domain`**:
   * Projektets kärna. Innehåller domänmodeller (`Ticket`, `Customer`, `Comment`), enums (`Status`, `Priority`) och affärsregler. Har inga beroenden till andra lager.
2. **`SupportCaseManagement.Application`**:
   * Hanterar användningsfall (use cases), logik och flöden. Innehåller DTOs, gränssnitt (t.ex. `ITicketRepository`) och tjänster för att koordinera mellan domänen och presentationen.
3. **`SupportCaseManagement.Infrastructure`**:
   * Hanterar teknisk persistering. Implementerar datalagring via lokala JSON-filer och sköter läsning/skrivning av data till disken.
4. **`SupportCaseManagement.Presentation`**:
   * Användargränssnittet byggt i WinUI 3 med MVVM-mönstret (`CommunityToolkit.Mvvm`). Innehåller XAML-vyer, ViewModels och inställningar för Dependency Injection (`Microsoft.Extensions.DependencyInjection`).

## Datalagring & Testdata (JSON)
- **Lagringsplats**: Applikationen sparar data lokalt i en JSON-fil (t.ex. `tickets.json` och `customers.json`) som skapas i användarens lokala appdata-mapp (`%LocalAppData%`) eller direkt i projektets körningsmapp (`bin/Debug/.../data/`).
- **Köras med egna exempeldata**: Eftersom applikationen läser in sin data från denna lokala JSON-fil vid uppstart kan du enkelt lägga till eller ersätta filen med din egen testdata (JSON-format). När appen startas om kommer den automatiskt att läsa in de nya ärendena och kunderna från den uppdaterade filen

## Resultat av manuell kontroll
Vi har genomfört manuella tester av hela applikationens användarflöde:
- **Navigering**: Verifierat att sidomenyn växlar smidigt mellan Översikt, Ärenden och Kunder.
- **CRUD-funktioner**: Testat att skapa nya kunder, skapa ärenden kopplade till kunder, redigera uppgifter samt lägga till kommentarer. All data sparas och läses in korrekt från JSON-filen.
- **Sök & Filter**: Kontrollerat att sökfältet och statusfiltren i ärendelistan uppdaterar vyn direkt utan fel.
- **Inmatning & Validering**: Testat felaktig inmatning i formulär (t.ex. tomma fält eller felaktiga tecken) för att säkerställa att applikationen hanterar fel säkert (t.ex. via `TryParse`) utan att krascha.
- **Visuell kontroll**: Verifierat att färgkodningen på översikten (röd/gul/grön) stämmer överens med antalet ärenden i respektive status.


## AI-användning
I projektet har AI-verktyg som **Gemini** och **ChatGPT** använts som aktivt stöd under utvecklingen:
- **Testgenerering**: AI har använts för att generera flera av applikationens enhetstester.
- **Felsökning & Problemhantering**: Verktygen har använts löpande under programmeringen för att felsöka kod, lösa byggfel (t.ex. `NETSDK1112`) och stötta när utvecklarna stött på problem.
- **Planering & Förståelse**: Stöd för att strukturera uppgifter (som fördes över till Trello) samt förklara teoretiska begrepp (t.ex. DDD, LINQ, `TryParse` och Dependency Injection).

**Kontroll av AI-förslag**: 
Alla AI-genererade förslag, tester och koder har noggrant granskats, anpassats och testats manuellt av gruppen innan de integrerades i projektet. Gruppen ansvarar fullt ut för och förstår den kod och de tester som har lagts in.
---

## Kom igång (Bygga & Starta)

### Förutsättningar
* .NET 8.0 SDK (eller nyare)
* Visual Studio 2022 (med arbetsbelastningen *Windows application development* installerad) eller VS Code.

### Bygga och köra via Visual Studio
1. Öppna solution-filen `SupportCaseManagement.sln` i Visual Studio.
2. Sätt **`SupportCaseManagement.Presentation`** som startprojekt (Startup Project).
3. Tryck på **F5** eller klicka på **Start** för att bygga och starta applikationen.

### Bygga och köra via Terminal / VS Code
```bash
# Gå till Presentation-mappen
cd SupportCaseManagement.Presentation

# Återställ paket och kör appen
dotnet restore
dotnet run
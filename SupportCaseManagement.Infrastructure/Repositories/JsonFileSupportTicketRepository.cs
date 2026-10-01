using SupportCaseManagement.Domain.SupportTickets.Interfaces;
using SupportCaseManagement.Domain.SupportTickets.Models;
using System.Text.Json;

namespace SupportCaseManagement.Infrastructure.Repositories;

/// <summary>
/// Implementation av ISupportTicketRepository, som sparar och hämtar en lista med ärenden till och från en json-fil
/// </summary>
internal class JsonFileSupportTicketRepository : ISupportTicketRepository
{
    // skapar en filepath genom att kombinera flera delar
    private readonly string _supportTicketsFilePath = Path.Combine
    (
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SupportCaseManagement",
        "supportTickets.json"
    );

    // sätter "inställningar" för hur json filen ska sparas, sätter:
    // WriteIndented till true för att allt inte ska stå på en och samma rad,
    // PropertyNameCaseInsensitive till true för att den inte ska bry sig om det är stora eller små bokstäver
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Sparar den givna listan av ärenden i en json-fil med den förbestämda filpath:en.
    /// Den är async och använder sig av await för att applikationen inte ska låsas helt från all interaktion medans metoden körs.
    /// </summary>
    /// <param name="supportTickets">den lista av ärenden som ska sparas till json-fil</param>
    public async Task SaveAllAsync(IEnumerable<SupportTicket> supportTickets)
    {
        // CreateDirectory - skapar alla mappar och filen som står i filepath:en, om de inte redan existerar
        Directory.CreateDirectory(Path.GetDirectoryName(_supportTicketsFilePath)!);

        string json = JsonSerializer.Serialize(supportTickets, _options);
        var tempPath = _supportTicketsFilePath + ".tmp";

        // lägger ärendena i temp-filen först ifall något går fel, flyttar sen över till den riktiga filen när vi vet att det fungerade
        await File.WriteAllTextAsync(tempPath, json);
        File.Move(tempPath, _supportTicketsFilePath, overwrite: true);
    }

    /// <summary>
    /// Hämtar alla sparade ärenden från den förbestämda filepath:en och returnerar en lista med dessa om den lyckas.
    /// Den är async och använder await för att applikationen inte ska låsas helt från all interaktion medans metoden körs.
    /// </summary>
    /// <returns>en lista med alla sparade ärenden</returns>
    /// <exception cref="JsonException">om filen inte innehåller en json-fil, alltså om vi får tillbaka null, kastas en JsonExeption med medelande</exception>
    public async Task<List<SupportTicket>> GetAllAsync()
    {
        if (!File.Exists(_supportTicketsFilePath))
        {
            return [];
        }

        string json = await File.ReadAllTextAsync(_supportTicketsFilePath);

        List<SupportTicket> supportTickets = JsonSerializer.Deserialize <List<SupportTicket>>(json, _options)
            ?? throw new JsonException("Ärendelistans fil måste innehålla en json baserad lista");

        return supportTickets;
    }

    public async Task<SupportTicket> GetById(Guid id)
    {
        List<SupportTicket> supportTickets = await GetAllAsync();

        SupportTicket? supportTicket = supportTickets.FirstOrDefault(ticket => ticket.Id == id);

        if (supportTicket == null)
        {
            throw new ArgumentException($"Inget ärende med id '{id}' kunde hittas i den sparade filen");
        }

        return supportTicket;
    }

    public async Task<SupportTicket> Update(SupportTicket updatedTicket)
    {
        if (updatedTicket is null)
        {
            throw new ArgumentNullException("Ärendet kan inte ändras till null");
        }

        List<SupportTicket> supportTickets = await GetAllAsync();
        int index;

        for (index = 0; index < supportTickets.Count; index++)
        {
            if (supportTickets[index].Id == updatedTicket.Id)
            {
                supportTickets[index] = updatedTicket;
                break;
            }
        }

        await SaveAllAsync(supportTickets);

        return supportTickets[index];
    }
}

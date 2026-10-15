using SupportCaseManagement.Domain.Models;
using System.Text.Json;

namespace SupportCaseManagement.Application.Features.Settings;

public class SettingsService : ISettingsService
{
    private readonly string _settingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SupportCaseManagement",
        "settings.json");

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    private AppSettings _settings;

    public string CustomerFilePath => _settings.CustomerFilePath;
    public string SupportTicketFilePath => _settings.SupportTicketFilePath;

    public SettingsService()
    {
        if (File.Exists(_settingsPath))
        {
            var json = File.ReadAllText(_settingsPath);

            _settings = JsonSerializer.Deserialize<AppSettings>(json, _options)
                ?? throw new JsonException("Inställningsfilen innehåller inga inställningar.");
        }
        else
        {
            _settings = new AppSettings();
        }
    }

    public async Task SetCustomerFilePathAsync(string filePath)
    {
        ValidatePath(filePath);
        var path = Path.GetFullPath(filePath.Trim());

        // Skapa nya inställningar men behåll den befintliga ärendesökvägen
        var settings = new AppSettings
        {
            CustomerFilePath = path,
            SupportTicketFilePath = _settings.SupportTicketFilePath
        };

        await SaveSettingsAsync(settings);
    }

    public async Task SetSupportTicketFilePathAsync(string filePath)
    {
        ValidatePath(filePath);
        var path = Path.GetFullPath(filePath.Trim());

        // Skapa nya inställningar men behåll den befintliga kundsökvägen
        var settings = new AppSettings
        {
            CustomerFilePath = _settings.CustomerFilePath,
            SupportTicketFilePath = path
        };

        await SaveSettingsAsync(settings);
    }

    private void ValidatePath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Ange en sökväg.");

        var path = filePath.Trim();

        if (!Path.IsPathFullyQualified(path))
            throw new ArgumentException("Ange en fullständig sökväg, exempelvis C:\\Data\\fil.json.");

        if (!string.Equals(Path.GetExtension(path), ".json", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Filen måste ha filändelsen .json.");
    }

    private async Task SaveSettingsAsync(AppSettings settings)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_settingsPath)!);

        var json = JsonSerializer.Serialize(settings, _options);
        var temporaryPath = _settingsPath + ".tmp";

        await File.WriteAllTextAsync(temporaryPath, json);
        File.Move(temporaryPath, _settingsPath, overwrite: true);

        _settings = settings;
    }
}
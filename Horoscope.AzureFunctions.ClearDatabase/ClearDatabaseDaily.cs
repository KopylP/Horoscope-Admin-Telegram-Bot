using Google.Cloud.Firestore;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Horoscope.AzureFunctions;

public class ClearDatabaseDaily
{
    private readonly ILogger _logger;
    private readonly FirestoreDb _firestoreDb;

    public ClearDatabaseDaily(ILoggerFactory loggerFactory, FirestoreDb firestoreDb)
    {
        _logger = loggerFactory.CreateLogger<ClearDatabaseDaily>();
        _firestoreDb = firestoreDb;
    }

    [Function("ClearDatabaseDaily")]
    public async Task Run([TimerTrigger("0 1 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"ClearDatabaseDaily trigger function executed at: {DateTime.UtcNow}");

        try
        {
            var deletedDocumentsCount = await DeleteDocumentsFromYesterday();
            _logger.LogInformation($"{deletedDocumentsCount} documents were deleted.");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
    
    private async Task<int> DeleteDocumentsFromYesterday()
    {
        var yesterdayDate = DateTime.UtcNow.AddDays(-1).ToString("dd.MM.yyyy");
        var collection = _firestoreDb.Collection("Horoscope");
        var query = collection.WhereEqualTo("Date", yesterdayDate);
        var snapshot = await query.GetSnapshotAsync();

        int documentsCount = 0;
        foreach (var document in snapshot.Documents)
        {
            await document.Reference.DeleteAsync();
            documentsCount++;
        }

        return documentsCount;
    }
}
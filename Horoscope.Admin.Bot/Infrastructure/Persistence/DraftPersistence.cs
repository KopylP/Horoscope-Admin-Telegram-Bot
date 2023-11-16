using Google.Cloud.Firestore;
using Horoscope.Admin.Bot.Framework.Persistence;

namespace Horoscope.Admin.Bot.Infrastructure.Persistence;

[FirestoreData]
public sealed class DraftPersistence : IFirebaseEntity
{
    public string Id { get; set; } = string.Empty;
    
    [FirestoreProperty]
    public string? Date { get; set; }
    
    [FirestoreProperty]
    public string? Sign { get; set; }
    
    [FirestoreProperty]
    public string? Foresight { get; set; }
}
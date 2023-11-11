using Google.Cloud.Firestore;
using Horoscope.Admin.Bot.Framework.Persistence;

namespace Horoscope.Admin.Bot.Persistence;

[FirestoreData]
public sealed class HoroscopePersistence : IFirebaseEntity
{
    [FirestoreProperty]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty] 
    public string Date { get; init; } = null!;

    [FirestoreProperty] 
    public string Sign { get; init; } = string.Empty;

    [FirestoreProperty] 
    public string[] Foresight { get; init; } = Array.Empty<string>();

    public bool IsAnyForesight => Foresight?.Any() ?? false;
}
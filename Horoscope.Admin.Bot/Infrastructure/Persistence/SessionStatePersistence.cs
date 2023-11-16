using Google.Cloud.Firestore;
using Horoscope.Admin.Bot.Framework.Persistence;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Infrastructure.Persistence;

[FirestoreData]
public sealed class SessionStatePersistence : IFirebaseEntity
{
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty] 
    public string CurrentState { get; init; } = string.Empty;

    public static SessionStatePersistence Create(string id, State currentState) => new()
    {
        Id = id,
        CurrentState = currentState.ToString()
    };
}


using Google.Cloud.Firestore;
using Horoscope.Admin.Bot.Framework.Contexts;
using Horoscope.Admin.Bot.Framework.Persistence;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Persistence;

[FirestoreData]
public sealed class SessionStatePersistence : IFirebaseEntity
{
    public string Id { get; set; }

    [FirestoreProperty] 
    public string CurrentState { get; set; } = string.Empty;

    public static SessionStatePersistence Create(string id, State currentState) => new()
    {
        Id = id,
        CurrentState = currentState.ToString()
    };
}


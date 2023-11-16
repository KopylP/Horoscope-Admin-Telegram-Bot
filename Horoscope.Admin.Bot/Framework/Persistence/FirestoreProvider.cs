using Google.Cloud.Firestore;
using Horoscope.Admin.Bot.Framework.Extensions;

namespace Horoscope.Admin.Bot.Framework.Persistence;

public class FirestoreProvider
{
    private readonly FirestoreDb _fireStoreDb;

    public FirestoreProvider(FirestoreDb fireStoreDb)
    {
        _fireStoreDb = fireStoreDb;
    }

    public async Task AddOrUpdate<T>(T entity, CancellationToken ct = default) where T : IFirebaseEntity
    {
        var document = _fireStoreDb.Collection(GetEntityName<T>())
            .Document(entity.Id);
        await document.SetAsync(entity, cancellationToken: ct);
    }
    
    public async Task AddOrUpdateBulk<T>(IEnumerable<T> entities, CancellationToken ct = default) where T : IFirebaseEntity
    {
        if (entities == null || !entities.Any())
        {
            throw new ArgumentException("Entities collection cannot be null or empty.");
        }

        var batch = _fireStoreDb.StartBatch();
    
        foreach (var entity in entities)
        {
            var documentReference = _fireStoreDb.Collection(GetEntityName<T>()).Document(entity.Id);
            batch.Set(documentReference, entity);
        }

        await batch.CommitAsync(ct);
    }

    public async Task<T?> Get<T>(string id, CancellationToken ct = default) where T : IFirebaseEntity
    {
        var document = _fireStoreDb.Collection(GetEntityName<T>()).Document(id);
        var snapshot = await document.GetSnapshotAsync(ct);
        return snapshot.ConvertTo<T>();
    }

    public async Task<IReadOnlyCollection<T>> GetAll<T>(CancellationToken ct = default) where T : IFirebaseEntity
    {
        var collection = _fireStoreDb.Collection(GetEntityName<T>());
        var snapshot = await collection.GetSnapshotAsync(ct);
        
        return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
    }

    public async Task<IReadOnlyCollection<T>> WhereEqualTo<T>(string fieldPath, object value, CancellationToken ct = default) where T : IFirebaseEntity
    {
        return await GetList<T>(_fireStoreDb.
            Collection(GetEntityName<T>())
            .WhereEqualTo(fieldPath, value), ct);
    }

    // just add here any method you need here WhereGreaterThan, WhereIn etc ...

    private static async Task<IReadOnlyCollection<T>> GetList<T>(Query query, CancellationToken ct) where T : IFirebaseEntity
    {
        var snapshot = await query.GetSnapshotAsync(ct);
        return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
    }

    private string GetEntityName<T>() where T : IFirebaseEntity
    {
        return typeof(T).Name.RemoveSubstringFromEnd("Persistence")!;
    }
}
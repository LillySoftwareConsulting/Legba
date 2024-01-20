using Legba.Engine.Models;
using LiteDB;
using System.Linq.Expressions;

namespace Legba.Engine.Services;

public class PromptRepository(string databasePath) : IDisposable
{
    // This will create a LiteDB database file in the same directory as the executable,
    // if one does not already exist.
    private readonly LiteDatabase _liteDb = new(databasePath);
    private bool _disposed = false;

    public static string GetCollectionName<T>() where T : PromptPrefix
    {
        return typeof(T).Name;
    }

    public void Add<T>(T item) where T : PromptPrefix
    {
        var collectionName = GetCollectionName<T>();
        var collection = _liteDb.GetCollection<T>(collectionName);

        // TODO: Possibly raise an exception if the Id is not Guid.Empty.
        // New items Id is Guid.Empty, so we need to generate a real Id.
        item.Id = Guid.NewGuid();

        collection.Insert(item);
    }

    public bool Update<T>(T item) where T : PromptPrefix
    {
        var collectionName = GetCollectionName<T>();
        var collection = _liteDb.GetCollection<T>(collectionName);

        return collection.Update(item);
    }

    public bool AddOrUpdate<T>(T item) where T : PromptPrefix
    {
        var collectionName = GetCollectionName<T>();
        var collection = _liteDb.GetCollection<T>(collectionName);

        if(item.Id == Guid.Empty)
        {
            // New items Id is Guid.Empty, so we need to generate a real Id.
            item.Id = Guid.NewGuid();
            collection.Insert(item);
            return true;
        }
        else
        {
            return collection.Update(item);
        }
    }

    public bool Delete<T>(Guid id) where T : PromptPrefix
    {
        var collectionName = GetCollectionName<T>();
        var collection = _liteDb.GetCollection<T>(collectionName);

        return collection.Delete(id);
    }

    public IEnumerable<T> Find<T>(Expression<Func<T, bool>> predicate) where T : PromptPrefix
    {
        var collectionName = GetCollectionName<T>();
        var collection = _liteDb.GetCollection<T>(collectionName);

        return collection.Find(predicate);
    }

    public IEnumerable<T> GetAll<T>() where T : PromptPrefix
    {
        var collectionName = GetCollectionName<T>();

        return _liteDb.GetCollection<T>(collectionName).FindAll();
    }

    #region Implementation of IDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed state (managed objects)
                _liteDb?.Dispose();
            }

            // Free unmanaged resources (unmanaged objects) and override finalizer
            // Set large fields to null

            _disposed = true;
        }
    }

    // Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~PromptRepository()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    #endregion
}
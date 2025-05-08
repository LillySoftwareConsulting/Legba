using LiteDB;

namespace Legba.Engine.Services;

public class PromptRepository(string databasePath) : IDisposable
{
    // This will create a LiteDB database file in the same directory as the executable,
    // if one does not already exist.
    private readonly LiteDatabase _liteDb = new(databasePath);
    private bool _disposed = false;

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
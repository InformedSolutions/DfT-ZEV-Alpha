using DfT.ZEV.Core.Domain.Manufacturers.Models;

namespace DfT.ZEV.Core.Domain.Manufacturers.Services;

/// <summary>
/// Represents a repository for managing manufacturers.
/// </summary>
public interface IManufacturerRepository
{
    /// <summary>
    /// Retrieves a manufacturer by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the manufacturer.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation with the retrieved manufacturer or null if not found.</returns>
    public ValueTask<Manufacturer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a manufacturer by its name asynchronously.
    /// </summary>
    /// <param name="name">The name of the manufacturer.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation with the retrieved manufacturer or null if not found.</returns>
    public ValueTask<Manufacturer?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all manufacturers asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation with a collection of all manufacturers.</returns>
    public ValueTask<IEnumerable<Manufacturer>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches all manufacturers asynchronously.
    /// </summary>
    /// <param name="term">Search term</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation with a collection of all manufacturers.</returns>
    public ValueTask<IEnumerable<Manufacturer>> SearchAsync(string term,CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paged collection of manufacturers asynchronously.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of manufacturers per page.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation with a paged collection of manufacturers.</returns>
    public ValueTask<IEnumerable<Manufacturer>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a collection of manufacturer names asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation with a collection of manufacturer names.</returns>
    public ValueTask<IEnumerable<string>> GetManufacturerNamesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserts a new manufacturer asynchronously.
    /// </summary>
    /// <param name="manufacturer">The manufacturer to insert.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task InsertAsync(Manufacturer manufacturer, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserts a collection of manufacturers asynchronously.
    /// </summary>
    /// <param name="manufacturers">The collection of manufacturers to insert.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task BulkInsertAsync(IList<Manufacturer> manufacturers, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing manufacturer.
    /// </summary>
    /// <param name="manufacturer">The manufacturer to update.</param>
    public void Update(Manufacturer manufacturer);

    /// <summary>
    /// Deletes an existing manufacturer.
    /// </summary>
    /// <param name="manufacturer">The manufacturer to delete.</param>
    public void Delete(Manufacturer manufacturer);
}

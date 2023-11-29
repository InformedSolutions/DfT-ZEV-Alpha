using AutoFixture;
using DfT.ZEV.Core.Domain.Accounts.Models;
using FluentAssertions;
using DfT.ZEV.Core.Infrastructure.Repositories;

namespace DfT.ZEV.Core.Infrastructure.Tests.Repositories;

[TestFixture]
internal class ManufacturerRepositoryTests : BaseRepositoryTest<ManufacturerRepository>
{
    [Test]
    public async Task GetByIdAsync_ExistingId_ShouldReturnManufacturer()
    {
        // Arrange
        var manufacturer = _fixture.Build<Manufacturer>().Create();
        _context.Manufacturers.Add(manufacturer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(manufacturer.Id);

        // Assert
        result.Should().BeEquivalentTo(manufacturer);
    }

    [Test]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistingId);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetByNameAsync_ExistingName_ShouldReturnManufacturer()
    {
        // Arrange
        var manufacturer = _fixture.Build<Manufacturer>().Create();
        _context.Manufacturers.Add(manufacturer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByNameAsync(manufacturer.Name);

        // Assert
        result.Should().BeEquivalentTo(manufacturer);
    }

    [Test]
    public async Task GetByNameAsync_NonExistingName_ShouldReturnNull()
    {
        // Arrange
        var nonExistingName = "NonExistingManufacturer";

        // Act
        var result = await _repository.GetByNameAsync(nonExistingName);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllManufacturers()
    {
        // Arrange
        var manufacturers = _fixture.Build<Manufacturer>().CreateMany(5).ToList();
        _context.Manufacturers.AddRange(manufacturers);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(manufacturers);
    }

    [Test]
    public async Task GetPagedAsync_ShouldReturnPagedManufacturers()
    {
        // Arrange
        var manufacturers = _fixture.Build<Manufacturer>().CreateMany(15).ToList();
        _context.Manufacturers.AddRange(manufacturers);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPagedAsync(0, 10);

        // Assert
        result.Should().BeEquivalentTo(manufacturers.Skip(0).Take(10));
    }

    [Test]
    public async Task InsertAsync_ShouldInsertManufacturer()
    {
        // Arrange
        var manufacturer = _fixture.Build<Manufacturer>().Create();

        // Act
        await _repository.InsertAsync(manufacturer);
        await _context.SaveChangesAsync();

        // Assert
        _context.Manufacturers.Should().ContainEquivalentOf(manufacturer);
    }

    [Test]
    public void Update_ShouldUpdateManufacturer()
    {
        // Arrange
        var manufacturer = _fixture.Build<Manufacturer>().Create();
        _context.Manufacturers.Add(manufacturer);
        _context.SaveChanges();
        manufacturer.WithName("UpdatedManufacturer");
        // Act
        _repository.Update(manufacturer);
        _context.SaveChanges();

        // Assert
        _context.Manufacturers.Single().Name.Should().Be("UpdatedManufacturer");
    }

    [Test]
    public void Delete_ShouldRemoveManufacturer()
    {
        // Arrange
        var manufacturer = _fixture.Build<Manufacturer>().Create();
        _context.Manufacturers.Add(manufacturer);
        _context.SaveChanges();

        // Act
        _repository.Delete(manufacturer);
        _context.SaveChanges();

        // Assert
        _context.Manufacturers.Should().BeEmpty();
    }
}
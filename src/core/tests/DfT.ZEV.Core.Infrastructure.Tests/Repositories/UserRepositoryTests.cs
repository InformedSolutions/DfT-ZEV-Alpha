using AutoFixture;
using DfT.ZEV.Core.Domain.Accounts.Models;
using DfT.ZEV.Core.Infrastructure.Persistence;
using DfT.ZEV.Core.Infrastructure.Repositories;
using FluentAssertions;

namespace DfT.ZEV.Core.Infrastructure.Tests.Repositories;

[TestFixture]
internal class UserRepositoryTests : BaseRepositoryTest<UserRepository>
{
    [Test]
    public async Task GetByIdAsync_ExistingId_ShouldReturnUser()
    {
        // Arrange
        var user = _fixture.Build<User>().Create();
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(user.Id);

        // Assert
        result.Should().BeEquivalentTo(user);
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
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = _fixture.CreateMany<User>(5).ToList();
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(users);
    }

    [Test]
    public async Task GetPagedAsync_ShouldReturnPagedUsers()
    {
        // Arrange
        var users = _fixture.CreateMany<User>(10).ToList();
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPagedAsync(1, 5);

        // Assert
        result.Should().BeEquivalentTo(users.OrderByDescending(x => x.CreatedAt).Skip(5).Take(5));
    }

    [Test]
    public async Task InsertAsync_ShouldInsertUser()
    {
        // Arrange
        var user = _fixture.Build<User>().Create();

        // Act
        await _repository.InsertAsync(user);
        await _context.SaveChangesAsync();

        // Assert
        _context.Users.Should().ContainEquivalentOf(user);
    }

    [Test]
    public async Task Delete_ShouldRemoveUser()
    {
        // Arrange
        var user = _fixture.Build<User>().Create();
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        _repository.Delete(user);
        await _context.SaveChangesAsync();

        // Assert
        var deletedUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
        deletedUser.Should().BeNull();
    }
    
    [Test]
    public async Task SaveChangesAsync_ShouldReturnNumberOfChanges()
    {
        // Arrange
        var user = _fixture.Build<User>().Create();
        _context.Users.Add(user);

        // Act
        var result = await _repository.SaveChangesAsync();

        // Assert
        result.Should().Be(1);
    }
}
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Zev.Core.Domain.Processes.Models;
using Zev.Core.Domain.Processes.Values;
using Zev.Core.Infrastructure.Persistence;
using Zev.Core.Infrastructure.Repositories;

namespace Zev.Core.Infrastructure.Tests.Repositories;

[TestFixture]
public class ProcessRepositoryTests
{
    private AppDbContext _context = null!;
    private ProcessRepository _processRepository = null!;
    private IFixture _fixture = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _processRepository = new ProcessRepository(_context);
        _fixture = new Fixture();
    }

    [Test]
    public async Task GetByIdAsync_WhenCalled_ShouldReturnProcess()
    {
        // Arrange
        var process = _fixture.Build<Process>()
            .Without(x => x.Metadata)
            .Without(x => x.Result)
            .Create();
        
        _context.Processes.Add(process);
        await _context.SaveChangesAsync();

        // Act
        var result = await _processRepository.GetByIdAsync(process.Id);

        // Assert
        result.Should().BeEquivalentTo(process);
    }

    [Test]
    public async Task GetPagedAsync_WhenCalled_ShouldReturnProcesses()
    {
        // Arrange
        var processes = _fixture.Build<Process>()
            .Without(x => x.Metadata)
            .Without(x => x.Result)
            .CreateMany(100).ToList();
        
        _context.Processes.AddRange(processes);
        await _context.SaveChangesAsync();

        // Act
        var result = await _processRepository.GetPagedAsync(1, 10);

        // Assert
        result.Should().BeEquivalentTo(processes.OrderByDescending(x => x.LastUpdated).Skip(10).Take(10));
    }

    [Test]
    public async Task AddAsync_WhenCalled_ShouldAddProcess()
    {
        // Arrange
        var process = _fixture.Build<Process>()
            .Without(x => x.Metadata)
            .Without(x => x.Result)
            .Create();

        // Act
        await _processRepository.AddAsync(process);
        await _context.SaveChangesAsync();

        // Assert
        _context.Processes.Should().ContainEquivalentOf(process);
    }

    [Test]
    public async Task Update_WhenCalled_ShouldUpdateProcess()
    {
        // Arrange
        var process = _fixture.Build<Process>()
            .Without(x => x.Metadata)
            .Without(x => x.Result)
            .Create();
        
        _context.Processes.Add(process);
        await _context.SaveChangesAsync();
        process.State = ProcessStateEnum.Failed;

        // Act
        _processRepository.Update(process);
        await _context.SaveChangesAsync();

        // Assert
        _context.Processes.Single().State.Should().Be(ProcessStateEnum.Failed);
    }

    [Test]
    public async Task Delete_WhenCalled_ShouldRemoveProcess()
    {
        // Arrange
        var process = _fixture.Build<Process>()
            .Without(x => x.Metadata)
            .Without(x => x.Result)
            .Create();
        _context.Processes.Add(process);
        await _context.SaveChangesAsync();

        // Act
        _processRepository.Delete(process);
        await _context.SaveChangesAsync();

        // Assert
        _context.Processes.Should().BeEmpty();
    }
}
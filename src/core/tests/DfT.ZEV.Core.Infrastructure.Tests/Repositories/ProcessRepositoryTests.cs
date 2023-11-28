using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using DfT.ZEV.Core.Domain.Processes.Models;
using DfT.ZEV.Core.Domain.Processes.Values;
using DfT.ZEV.Core.Infrastructure.Persistence;
using DfT.ZEV.Core.Infrastructure.Repositories;

namespace DfT.ZEV.Core.Infrastructure.Tests.Repositories;

[TestFixture]
internal class ProcessRepositoryTests : BaseRepositoryTest<ProcessRepository>
{
    
    
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
        var result = await _repository.GetByIdAsync(process.Id);

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
        var result = await _repository.GetPagedAsync(1, 10);

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
        await _repository.AddAsync(process);
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
        _repository.Update(process);
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
        _repository.Delete(process);
        await _context.SaveChangesAsync();

        // Assert
        _context.Processes.Should().BeEmpty();
    }
}
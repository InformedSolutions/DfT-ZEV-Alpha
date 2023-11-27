using DfT.ZEV.Core.Domain.Processes.Models;
using DfT.ZEV.Core.Domain.Processes.Services;
using DfT.ZEV.Core.Domain.Processes.Values;
using DfT.ZEV.Core.Infrastructure.Repositories;

namespace DfT.ZEV.Core.Application.Processes;

public class ProcessService : IProcessService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProcessService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<Process> CreateProcessAsync(Guid id, ProcessTypeEnum processType, CancellationToken cancellationToken = default)
    {
        var process = new Process(id, processType);
        await _unitOfWork.Processes.AddAsync(process, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return process;
    }

    public async ValueTask<Process> StartProcessAsync(Guid id, object? metadata = default, CancellationToken cancellationToken = default)
    {
        var process = await _unitOfWork.Processes.GetByIdAsync(id, cancellationToken);
        if (process is null)
            throw new Exception($"Process with id {id} not found.");
        
        process.Start(metadata);
        _unitOfWork.Processes.Update(process);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return process;
    }

    public async ValueTask<Process> FinishProcessAsync(Guid id, object? result = default, CancellationToken cancellationToken = default)
    {
        var process = await _unitOfWork.Processes.GetByIdAsync(id, cancellationToken);
        if (process is null)
            throw new Exception($"Process with id {id} not found.");
        
        process.Finish(result);
        _unitOfWork.Processes.Update(process);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return process;
    }

    public async ValueTask<Process> FailProcessAsync(Guid id, object? result = default, CancellationToken cancellationToken = default)
    {
       var process = await _unitOfWork.Processes.GetByIdAsync(id, cancellationToken);
       if(process is null)
           throw new Exception($"Process with id {id} not found.");
       
       process.Fail(result);
       
       _unitOfWork.Processes.Update(process);
       await _unitOfWork.SaveChangesAsync(cancellationToken);

       return process;
    }
}
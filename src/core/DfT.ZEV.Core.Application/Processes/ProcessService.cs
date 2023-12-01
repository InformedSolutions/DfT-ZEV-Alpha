using DfT.ZEV.Core.Application.Processes.Exceptions;
using DfT.ZEV.Core.Domain.Abstractions;
using DfT.ZEV.Core.Domain.Processes.Models;
using DfT.ZEV.Core.Domain.Processes.Services;
using DfT.ZEV.Core.Domain.Processes.Values;

namespace DfT.ZEV.Core.Application.Processes;

internal sealed class ProcessService : IProcessService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProcessService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<Process> CreateProcessAsync(Guid id, ProcessType processType, CancellationToken cancellationToken = default)
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
            throw ProcessExceptions.NotFound(id);
        
        process.Start(metadata);
        _unitOfWork.Processes.Update(process);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return process;
    }

    public async ValueTask<Process> FinishProcessAsync(Guid id, object? result = default, CancellationToken cancellationToken = default)
    {
        var process = await _unitOfWork.Processes.GetByIdAsync(id, cancellationToken);
        if (process is null)
            throw ProcessExceptions.NotFound(id);
        
        process.Finish(result);
        _unitOfWork.Processes.Update(process);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return process;
    }

    public async ValueTask<Process> FailProcessAsync(Guid id, object? result = default, CancellationToken cancellationToken = default)
    {
       var process = await _unitOfWork.Processes.GetByIdAsync(id, cancellationToken);
       if(process is null)
           throw ProcessExceptions.NotFound(id);
       
       process.Fail(result);
       
       _unitOfWork.Processes.Update(process);
       await _unitOfWork.SaveChangesAsync(cancellationToken);

       return process;
    }
}
using Apollo77.Shared;
using Apollo77.Shared.Util;
using Apollo77.Core.CoreServiceProvider;

using System.Collections.Generic;

namespace Apollo77.Api.ApiService;

internal class ProcessApi(IProcessProvider _processesFinder) : IProcessApi
{
    public Response<List<ProcessInfo>> GetAllRunningProcessess()
    {
        return _processesFinder.GetAllProcesses();
    }

    public Response<bool> IsRunningProcessByFilePath(string exePath)
    {
        return _processesFinder.IsRunningProcessByFilePath(exePath);
    }

    public Response<bool> IsProcessRunningById(int processId)
    {
        return _processesFinder.IsProcessRunningById(processId);
    }
}
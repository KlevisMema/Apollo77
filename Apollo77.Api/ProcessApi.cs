using Apollo77.CORE;
using Apollo77.Shared;
using Apollo77.Shared.Util;

using System.Collections.Generic;

namespace Apollo77.Api;

public class ProcessApi(IProcessesFinder _processesFinder) : IProcessApi
{
    public Response<List<ProcessInfo>> GetAllRunningProcessess()
    {
        return _processesFinder.GetAllProcesses();
    }
}
using Apollo77.Shared;
using Apollo77.Shared.Util;

using System.Collections.Generic;

namespace Apollo77.Api.ApiService;

public interface IProcessApi
{
    Response<bool> OpenHandlerToProcess(int processId);
    Response<bool> IsRunningProcessById(int processId);
    Response<List<ProcessInfo>> GetAllRunningProcessess();
    Response<bool> IsRunningProcessByFilePath(string exePath);
}
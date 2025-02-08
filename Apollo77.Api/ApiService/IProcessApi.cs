using Apollo77.Shared;
using Apollo77.Shared.Util;

using System.Collections.Generic;

namespace Apollo77.Api.ApiService;

public interface IProcessApi
{
    Response<bool> OpenHandlerToProcess(int processId);
    Response<bool> IsRunningProcessById(int processId);
    Response<bool> IsRunningProcessByFilePath(string exePath);
    Response<IEnumerable<ProcessInfo>> GetRunningApplications();
    Response<IEnumerable<ProcessInfo>> GetAllRunningProcessess();
}
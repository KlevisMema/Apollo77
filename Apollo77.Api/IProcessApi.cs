using Apollo77.Shared;
using Apollo77.Shared.Util;
using System.Collections.Generic;

namespace Apollo77.Api
{
    public interface IProcessApi
    {
        Response<List<ProcessInfo>> GetAllRunningProcessess();
    }
}
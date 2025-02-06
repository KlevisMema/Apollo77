using Apollo77.Shared;
using Apollo77.Shared.Util;

using Microsoft.UI.Xaml.Media.Imaging;

using System.Diagnostics;
using System.Collections.Generic;

namespace Apollo77.Core.CoreServiceProvider;

public interface IProcessProvider
{
    Response<List<ProcessInfo>> GetAllProcesses();
    Response<bool> IsProcessRunningById(int processId);
    Response<BitmapImage?> GetProcessIcon(Process process);
    Response<bool> IsRunningProcessByFilePath(string exePath);
}
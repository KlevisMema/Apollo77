using Apollo77.Shared;
using Apollo77.Shared.Util;

using Microsoft.UI.Xaml.Media.Imaging;

using System.Diagnostics;
using System.Collections.Generic;

namespace Apollo77.CORE;

public interface IProcessesFinder
{
    Response<List<ProcessInfo>> GetAllProcesses();
    Response<BitmapImage?> GetProcessIcon(Process process);
}
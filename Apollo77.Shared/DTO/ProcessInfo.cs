using Microsoft.UI.Xaml.Media.Imaging;

namespace Apollo77.Shared;

public class ProcessInfo
{
    public string Name { get; set; } = string.Empty;
    public int Id { get; set; }
    public BitmapImage? Icon { get; set; }
    public string MemoryUsage { get; set; } = string.Empty;
    public string CpuTime { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string ThreadsCount { get; set; } = string.Empty;
}
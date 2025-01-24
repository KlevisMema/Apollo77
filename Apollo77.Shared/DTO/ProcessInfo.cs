using Microsoft.UI.Xaml.Media.Imaging;

namespace Apollo77.Shared;

public class ProcessInfo
{
    public string Name { get; set; } = string.Empty;
    public int Id { get; set; }
    public BitmapImage? Icon { get; set; }
}
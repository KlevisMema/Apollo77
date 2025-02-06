using Microsoft.UI.Xaml.Controls;

namespace Apollo77.UI.Controls;

public sealed partial class RunningProcessesInfo : UserControl
{
    public int NumberOfRunningProcesses { get; set; }

    public RunningProcessesInfo()
    {
        this.InitializeComponent();
    }
}
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
namespace Apollo77.UI.Controls;

public sealed partial class RunningProcessesInfo : UserControl
{
    public int NumberOfRunningProccesses { get; set; }

    public RunningProcessesInfo()
    {
        this.InitializeComponent();
    }
}
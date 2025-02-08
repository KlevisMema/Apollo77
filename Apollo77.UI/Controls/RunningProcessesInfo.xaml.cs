using Microsoft.UI.Xaml.Controls;

using System.ComponentModel;

namespace Apollo77.UI.Controls;

public sealed partial class RunningProcessesInfo : UserControl, INotifyPropertyChanged
{
    public int NumberOfRunningProcesses { get; set; }
    public string TextBlockName { get; set; } = string.Empty;

    public RunningProcessesInfo()
    {
        this.InitializeComponent();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
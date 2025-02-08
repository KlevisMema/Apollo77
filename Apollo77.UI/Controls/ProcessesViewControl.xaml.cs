using Apollo77.Shared;
using Apollo77.UI.Enums;
using Apollo77.UI.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;

using System.ComponentModel;

namespace Apollo77.UI.Controls;

internal sealed partial class ProcessesViewControl : UserControl, INotifyPropertyChanged
{
    public ProcessesViewControlViewModel ViewModel { get; }

    public ProcessesViewControl()
    {
        this.InitializeComponent();
        ViewModel = (App.Current as App)!._host.Services.GetRequiredService<ProcessesViewControlViewModel>();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void LoadData(ProcessCategory processCategory)
    {
        this.ViewModel.LoadProcesses(processCategory);
    }

    private void ProcessListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.ProcessListBox.SelectedItem is ProcessInfo selectedProcess)
        {
            this.ViewModel.ProcessListBoxSelectionChanged(selectedProcess);
        }
    }

    public void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Apollo77.Api;
using Apollo77.Shared;

using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Data;
using Apollo77.UI.Controls;

namespace Apollo77.UI;

public sealed partial class MainWindow : Window
{
    private readonly IProcessApi _processApi;

    private ObservableCollection<ProcessInfo> Processes { get; set; } = default!;
    private ObservableCollection<ProcessInfo> AllProcesses { get; set; } = default!;

    public MainWindow(IProcessApi processApi)
    {
        this.InitializeComponent();
        this._processApi = processApi;
        this.InitializePrivateObjects();
        this.GetRunninngProcessess();
    }

    private void InitializePrivateObjects()
    {
        Processes = [];
        AllProcesses = [];
    }

    private void GetRunninngProcessess()
    {
        var getRunninngProcessessResult = _processApi.GetAllRunningProcessess();

        if (getRunninngProcessessResult.Succsess && getRunninngProcessessResult.Value is not null)
        {
            AllProcesses = new ObservableCollection<ProcessInfo>(getRunninngProcessessResult.Value);
            Processes = new ObservableCollection<ProcessInfo>(getRunninngProcessessResult.Value);
            ProcessListBox.ItemsSource = Processes;
            RunningProcessesInfo.NumberOfRunningProccesses = AllProcesses.Count;
        }
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = SearchBox.Text.ToLower();

        Processes.Clear();

        foreach (var process in AllProcesses)
        {
            if (process.Name.Contains(searchText, System.StringComparison.CurrentCultureIgnoreCase) || process.Id.ToString().Contains(searchText))
            {
                Processes.Add(process);
            }
        }
    }
}
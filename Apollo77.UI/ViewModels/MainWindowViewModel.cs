using Apollo77.Shared;
using Apollo77.Shared.Util;

using Microsoft.Extensions.Logging;

using System.ComponentModel;
using System.Collections.Generic;

using Apollo77.UI.Services;
using Apollo77.Api.ApiService;

namespace Apollo77.UI.ViewModels;

internal partial class MainWindowViewModel : INotifyPropertyChanged
{
    public readonly DialogService _dialogService;
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly AppState _appState;
    private readonly IProcessApi _processApi;
    private readonly ILogger<MainWindowViewModel> _logger;

    public MainWindowViewModel(
        IProcessApi processApi,
        ILogger<MainWindowViewModel> logger,
        DialogService dialogService,
        AppState appState
    )
    {
        this._logger = logger;
        this._appState = appState;
        this._processApi = processApi;
        this._dialogService = dialogService;

        this.GetRunningProcesses();
    }

    public void GetRunningProcesses()
    {
        Response<IEnumerable<ProcessInfo>> getRunninngProcessessResult = this._processApi.GetAllRunningProcessess();

        if (getRunninngProcessessResult.Succsess && getRunninngProcessessResult.Value is not null)
        {
            this._appState.SetAllRunningProcesses([.. getRunninngProcessessResult.Value]);
        }

        Response<IEnumerable<ProcessInfo>> getRunninngApplications = this._processApi.GetRunningApplications();

        if (getRunninngApplications.Succsess && getRunninngApplications.Value is not null)
        {
            this._appState.SetApplications([.. getRunninngApplications.Value]);
        }
    }

    public void SearchProcesses(string searchText)
    {
        //if (string.IsNullOrEmpty(searchText))
        //{
        //    this.Processes = new ObservableCollection<ProcessInfo>(AllProcesses);
        //    return;
        //}

        //IEnumerable<ProcessInfo> filtered = AllProcesses.Where(p =>
        //     p.Name.Contains(searchText, System.StringComparison.CurrentCultureIgnoreCase) ||
        //     p.Id.ToString().Contains(searchText));

        //this.Processes = new ObservableCollection<ProcessInfo>(filtered);
        //OnPropertyChanged(nameof(this.Processes));
    }

    public Response<bool> IsRunningProcessByFilePath(string filePath)
    {
        return this._processApi.IsRunningProcessByFilePath(filePath);
    }

    public void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public int GetNumberOfAllRunningProcesses()
    {
        return this._appState.GetAllRunningProcesses().Count;
    }

    public int GetNumberOfApplications()
    {
        return this._appState.GetApplications().Count;
    }
}
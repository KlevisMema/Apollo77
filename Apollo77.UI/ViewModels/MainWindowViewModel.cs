using Apollo77.Shared;
using Apollo77.Shared.Util;

using Microsoft.Extensions.Logging;

using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Apollo77.UI.Services;
using Apollo77.Api.ApiService;

namespace Apollo77.UI.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public int NumberOfRunningProcesses;
    public readonly DialogService _dialogService;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<ProcessInfo> Processes { get; set; } = default!;
    public ObservableCollection<ProcessInfo> AllProcesses { get; set; } = default!;

    private readonly IProcessApi _processApi;
    private readonly ILogger<MainWindowViewModel> _logger;

    public MainWindowViewModel(IProcessApi processApi, ILogger<MainWindowViewModel> logger, DialogService dialogService)
    {
        _processApi = processApi;
        _logger = logger;
        _dialogService = dialogService;
        InitializePrivateObjects();
        GetRunningProcesses();
    }

    private void InitializePrivateObjects()
    {
        this.Processes = [];
        this.AllProcesses = [];
    }

    public void GetRunningProcesses()
    {
        Response<List<ProcessInfo>> getRunninngProcessessResult = this._processApi.GetAllRunningProcessess();

        if (getRunninngProcessessResult.Succsess && getRunninngProcessessResult.Value is not null)
        {
            this.AllProcesses = new ObservableCollection<ProcessInfo>(getRunninngProcessessResult.Value);
            this.Processes = new ObservableCollection<ProcessInfo>(getRunninngProcessessResult.Value);
            NumberOfRunningProcesses = this.AllProcesses.Count;
        }
    }

    public void SearchProcesses(string searchText)
    {
        if (string.IsNullOrEmpty(searchText))
        {
            Processes = new ObservableCollection<ProcessInfo>(AllProcesses);
            return;
        }

        var filtered = AllProcesses.Where(p =>
             p.Name.Contains(searchText, System.StringComparison.CurrentCultureIgnoreCase) ||
             p.Id.ToString().Contains(searchText));

        Processes = new ObservableCollection<ProcessInfo>(filtered);
        OnPropertyChanged(nameof(Processes));
    }

    public Response<bool> IsRunningProcessByFilePath(string filePath)
    {
        return _processApi.IsRunningProcessByFilePath(filePath);
    }

    public void ProcessListBoxSelectionChanged(ProcessInfo selectedProcess)
    {
        Response<bool> isRunningProcessResult = _processApi.IsRunningProcessById(selectedProcess.Id);

        if (!isRunningProcessResult.Succsess)
        {
            _dialogService.ShowDialogAsync(title: "Something went wrong!", message: $"Error id: {isRunningProcessResult.ErrorId!}");
            return;
        }

        if (isRunningProcessResult.Succsess && !isRunningProcessResult.Value)
        {
            _dialogService.ShowDialogAsync(title: "Fail!", message: isRunningProcessResult.Message!);
            return;
        }

        OpenHanderToValidSelectedProcess(selectedProcess.Id);
    }

    public void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OpenHanderToValidSelectedProcess(int processId)
    {
        Response<bool> openHandlerToProcessResult = _processApi.OpenHandlerToProcess(processId);

        if (!openHandlerToProcessResult.Succsess)
        {
            _dialogService.ShowDialogAsync(title: "Something went wrong!", message: $"Error id: {openHandlerToProcessResult.ErrorId!}");
            return;
        }

        if (openHandlerToProcessResult.Succsess && !openHandlerToProcessResult.Value)
        {
            _dialogService.ShowDialogAsync(title: "Fail!", message: openHandlerToProcessResult.Message!);
            return;
        }
    }
}
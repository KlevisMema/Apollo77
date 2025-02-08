using Apollo77.Shared;
using Apollo77.UI.Enums;
using Apollo77.Shared.Util;
using Apollo77.UI.Services;
using Apollo77.Api.ApiService;

using Microsoft.Extensions.Logging;

using System.Collections.Generic;

namespace Apollo77.UI.ViewModels;

internal partial class ProcessesViewControlViewModel
{
    public List<ProcessInfo> Processes { get; set; } = [];

    private readonly AppState _appState;
    private readonly IProcessApi _processApi;
    private readonly DialogService _dialogService;
    private readonly ILogger<ProcessesViewControlViewModel> _logger;

    public ProcessesViewControlViewModel(
        AppState appState,
        IProcessApi processApi,
        DialogService dialogService,
        ILogger<ProcessesViewControlViewModel> logger
    )
    {
        this._logger = logger;
        this._appState = appState;
        this._processApi = processApi;
        this._dialogService = dialogService;
    }

    public void LoadProcesses(ProcessCategory processCategory)
    {
        List<ProcessInfo> processes = processCategory switch
        {
            ProcessCategory.RunningProcesses => _appState.GetAllRunningProcesses(),
            ProcessCategory.Applications => _appState.GetApplications(),
            _ => []
        };

        this.Processes.Clear();
        this.Processes = processes;
    }

    public void ProcessListBoxSelectionChanged(ProcessInfo selectedProcess)
    {
        Response<bool> isRunningProcessResult = this._processApi.IsRunningProcessById(selectedProcess.Id);

        if (!isRunningProcessResult.Succsess)
        {
            this._dialogService.ShowDialogAsync(title: "Something went wrong!", message: $"Error id: {isRunningProcessResult.ErrorId!}");
            return;
        }

        if (isRunningProcessResult.Succsess && !isRunningProcessResult.Value)
        {
            this._dialogService.ShowDialogAsync(title: "Fail!", message: isRunningProcessResult.Message!);
            return;
        }

        OpenHanderToValidSelectedProcess(selectedProcess.Id);
    }

    private void OpenHanderToValidSelectedProcess(int processId)
    {
        Response<bool> openHandlerToProcessResult = _processApi.OpenHandlerToProcess(processId);

        if (!openHandlerToProcessResult.Succsess)
        {
            this._dialogService.ShowDialogAsync(title: "Something went wrong!", message: $"Error id: {openHandlerToProcessResult.ErrorId!}");
            return;
        }

        if (openHandlerToProcessResult.Succsess && !openHandlerToProcessResult.Value)
        {
            this._dialogService.ShowDialogAsync(title: "Fail!", message: openHandlerToProcessResult.Message!);
            return;
        }
    }
}
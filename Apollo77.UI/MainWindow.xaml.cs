using Apollo77.UI.Pages;
using Apollo77.UI.Enums;
using Apollo77.Shared.Util;
using Apollo77.UI.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.ComponentModel;
using System.Threading.Tasks;

using WinRT.Interop;

using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml.Navigation;

namespace Apollo77.UI;

internal sealed partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindowViewModel ViewModel { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool PaneOpenend { get; set; } = true;

    public MainWindow(MainWindowViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
        SelectorBar_SelectionChanged(SelectorBar, null!);
        RunningProcessesInfo.TextBlockName = "All Running Processes";
    }

    private async void AddProcess_Click(object sender, RoutedEventArgs e)
    {
        StorageFile file = await PickProcessFileAsync();

        if (file != null)
        {
            Response<bool> isRunningProcessResult = this.ViewModel.IsRunningProcessByFilePath(file.Path);
            this.IsRunningProcessResult(isRunningProcessResult);
        }
        else
        {
            this.OpenDialog("Cancelled", "No file was selected.");
        }
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox? searchBox = sender as TextBox;
        this.ViewModel.SearchProcesses(searchBox?.Text ?? string.Empty);
    }

    private void TogglePane_Click(object sender, RoutedEventArgs e)
    {
        this.PaneOpenend = !this.PaneOpenend;
        this.OnPropertyChanged(nameof(this.PaneOpenend));
    }

    private void OpenDialog(string title, string content)
    {
        this.ViewModel._dialogService.ShowDialogAsync(title, content);
    }

    private void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async Task<StorageFile> PickProcessFileAsync()
    {
        FileOpenPicker picker = new()
        {
            SuggestedStartLocation = PickerLocationId.ComputerFolder,
        };

        picker.FileTypeFilter.Add(".exe");

        InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));

        StorageFile file = await picker.PickSingleFileAsync();

        return file;
    }

    private void IsRunningProcessResult(Response<bool> response)
    {
        if (response.Value)
        {
            this.OpenDialog("Success", "Process succsessfully added!");
            return;
        }

        if (response.Succsess && !response.Value)
        {
            this.OpenDialog("Fail", "Process is not running!");
            return;
        }

        if (!response.Succsess)
        {
            this.OpenDialog("Error", "Something went wrong!");
            return;
        }
    }

    private void SelectorBar_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        FrameNavigationOptions navOptions = new();

        if (args is not null)
        {
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
        }

        if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
        {
            navOptions.IsNavigationStackEnabled = false;
        }

        Type pageType = typeof(ProcessListPage);

        if (args is not null &&
            args.SelectedItem is NavigationViewItem selectedItem &&
            selectedItem.Tag is ProcessCategory category
        )
        {
            switch (category)
            {
                case ProcessCategory.RunningProcesses:
                    RunningProcessesInfo.TextBlockName = "All Running Processes";
                    RunningProcessesInfo.NumberOfRunningProcesses = ViewModel.GetNumberOfAllRunningProcesses();
                    break;
                case ProcessCategory.Applications:
                    RunningProcessesInfo.TextBlockName = "Applications";
                    RunningProcessesInfo.NumberOfRunningProcesses = ViewModel.GetNumberOfApplications();
                    break;
                default:
                    RunningProcessesInfo.TextBlockName = "All Running Processes";
                    RunningProcessesInfo.NumberOfRunningProcesses = ViewModel.GetNumberOfAllRunningProcesses();
                    break;
            }

            this.RunningProcessesInfo.OnPropertyChanged(nameof(RunningProcessesInfo.TextBlockName));
            this.RunningProcessesInfo.OnPropertyChanged(nameof(RunningProcessesInfo.NumberOfRunningProcesses));
            this.ContentFrame.NavigateToType(pageType, category, navOptions);
        }
    }
}
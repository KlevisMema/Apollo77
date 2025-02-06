using Apollo77.Shared;
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

namespace Apollo77.UI;

public sealed partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindowViewModel ViewModel { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool PaneOpenend { get; set; } = true;

    public MainWindow(MainWindowViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
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
            OpenDialog("Cancelled", "No file was selected.");
        }
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchBox = sender as TextBox;
        ViewModel.SearchProcesses(searchBox?.Text ?? string.Empty);
    }

    private void TogglePane_Click(object sender, RoutedEventArgs e)
    {
        PaneOpenend = !PaneOpenend;
        OnPropertyChanged(nameof(PaneOpenend));
    }

    private void ProcessListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ProcessListBox.SelectedItem is ProcessInfo selectedProcess)
        {
            this.ViewModel.ProcessListBoxSelectionChanged(selectedProcess);
        }
    }

    private async void OpenDialog(string title, string content)
    {
        ContentDialog dialog = new()
        {
            Title = title,
            Content = content,
            CloseButtonText = "OK",
            XamlRoot = this.Content.XamlRoot
        };

        await dialog.ShowAsync();
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
            OpenDialog("Success", "Process succsessfully added!");
            return;
        }

        if (response.Succsess && !response.Value)
        {
            OpenDialog("Fail", "Process is not running!");
            return;
        }

        if (!response.Succsess)
        {
            OpenDialog("Error", "Something went wrong!");
            return;
        }
    }
}
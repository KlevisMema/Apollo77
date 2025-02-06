using System;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace Apollo77.UI.Services;

public class DialogService
{
    public async void ShowDialogAsync(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = ((App.Current as App)!.m_window as Window)?.Content.XamlRoot
        };

        await dialog.ShowAsync();
    }
}
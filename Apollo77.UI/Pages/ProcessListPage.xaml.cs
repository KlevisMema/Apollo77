using Apollo77.UI.Enums;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System;

namespace Apollo77.UI.Pages;

public sealed partial class ProcessListPage : Page
{
    public ProcessListPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        string? category = e.Parameter.ToString();

        if (!String.IsNullOrEmpty(category))
        {
            this.ProcessesViewControl.LoadData(Enum.Parse<ProcessCategory>(category));
        }
        else
        {
            throw new ArgumentNullException(category, "Process category was empty or null");
        }
    }
}
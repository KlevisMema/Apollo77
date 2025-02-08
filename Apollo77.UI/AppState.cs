using Apollo77.Shared;

using System.Collections.Generic;

namespace Apollo77.UI;

internal class AppState
{
    private List<ProcessInfo> AllRunningProcesses = [];
    private List<ProcessInfo> Applications = [];

    public void SetAllRunningProcesses(List<ProcessInfo> allRunningProcesses)
    {
        this.AllRunningProcesses = allRunningProcesses;
    }

    public List<ProcessInfo> GetAllRunningProcesses()
    {
        return this.AllRunningProcesses;
    }

    public void SetApplications(List<ProcessInfo> applications)
    {
        this.Applications = applications;
    }

    public List<ProcessInfo> GetApplications()
    {
        return this.Applications;
    }
}
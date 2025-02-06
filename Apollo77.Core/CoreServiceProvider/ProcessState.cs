using Apollo77.Core.Constants;
using Apollo77.Core.Util;

namespace Apollo77.Core.CoreServiceProvider;

internal class ProcessState
{
    private nint processHandle;
    private int processId;

    public bool AttachToProcess(int pid)
    {
        processHandle = ExternalDll.OpenProcess(ProcessActions.PROCESS_ALL_ACCESS, false, pid);

        if (processHandle == nint.Zero)
        {
            return false;
        }

        processId = pid;
        return true;
    }

    public void Detach()
    {
        processHandle = nint.Zero;
        processId = 0;
    }

    public nint GetProcessHandle() => processHandle;
}
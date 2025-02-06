using System;

namespace Apollo77.Core.CoreServiceProvider;

internal class ProcessState
{
    private IntPtr processHandle;
    private int processId;

    public void SetProcessHandleProcessIdPtr(IntPtr processIdPtr)
    {
        this.processHandle = processIdPtr;
    }

    public void SetProcessId(int processId)
    {
        this.processId = processId;
    }

    public void Detach()
    {
        processHandle = IntPtr.Zero;
        processId = 0;
    }

    public IntPtr GetProcessHandle() => processHandle;
    public IntPtr GetProcessId() => processId;
}
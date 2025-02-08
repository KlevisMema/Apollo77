using Apollo77.Shared;
using Apollo77.Shared.Util;
using Apollo77.Core.Constants;

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Media.Imaging;

using Apollo77.Core.Util;

namespace Apollo77.Core.CoreServiceProvider;

internal class ProcessProvider(ILogger<ProcessProvider> _logger, ProcessState _processState) : IProcessProvider
{
    public Response<IEnumerable<ProcessInfo>> GetAllProcesses()
    {
        List<ProcessInfo> processList = [];

        foreach (Process process in Process.GetProcesses())
        {
            try
            {
                if (process.HasExited)
                {
                    continue;
                }

                processList.Add(new ProcessInfo
                {
                    Name = process.ProcessName,
                    Id = process.Id,
                    Icon = GetProcessIcon(process).Value,
                    MemoryUsage = (process.WorkingSet64 / 1024 / 1024).ToString(),
                    CpuTime = process.TotalProcessorTime.ToString("g"),
                    StartTime = process.StartTime.ToString("g"),
                    ThreadsCount = process.Threads.Count.ToString(),
                });
            }
            catch (Exception ex)
            {
                string exceptionId = GenerateGuid.New();

                _logger.LogError(message: ErrorMessages.GenericExceptionMessage, [ex, exceptionId]);

                continue;
            }
        }

        processList = OrderByIcon(processList).ToList();

        return Response<IEnumerable<ProcessInfo>>.SetResponse(processList, true, null, null);
    }

    public Response<BitmapImage?> GetProcessIcon(Process process)
    {
        try
        {
            string? filePath = process.MainModule?.FileName;

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(process), ErrorMessages.NullProcessFile);
            }

            Icon? icon = Icon.ExtractAssociatedIcon(filePath);

            if (icon == null)
            {
                return Response<BitmapImage?>.SetResponse(null, true, ErrorMessages.ProcessHasNoIcon, null);
            }

            using MemoryStream memory = new();
            icon.ToBitmap().Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;

            BitmapImage bitmapImage = new();
            bitmapImage.SetSource(memory.AsRandomAccessStream());

            return Response<BitmapImage?>.SetResponse(bitmapImage, true, null, null);
        }
        catch (Exception ex)
        {
            string exceptionId = GenerateGuid.New();

            _logger.LogError(message: ErrorMessages.GenericExceptionMessage, [ex, exceptionId]);

            return Response<BitmapImage?>.SetResponse(null, false, null, exceptionId);
        }
    }

    public Response<bool> IsRunningProcessById(int processId)
    {
        try
        {
            IntPtr hProcess = IntPtr.Zero;

            hProcess = ExternalDll.OpenProcess(ProcessActions.PROCESS_QUERY_INFORMATION | ProcessActions.PROCESS_VM_READ, false, processId);

            if (hProcess == IntPtr.Zero)
            {
                return Response<bool>.SetResponse(false, true, "Process is not running", null);
            }

            ExternalDll.CloseHandle(hProcess);

            return Response<bool>.SetResponse(true, true, null, null);
        }
        catch (Exception ex)
        {
            string exceptionId = GenerateGuid.New();

            _logger.LogError(message: ErrorMessages.GenericExceptionMessage, [ex, exceptionId]);

            return Response<bool>.SetResponse(false, false, null, exceptionId);
        }
    }

    public Response<bool> IsRunningProcessByFilePath(string exePath)
    {
        string? exceptionId = null;

        exePath = exePath.ToLowerInvariant();

        foreach (var process in Process.GetProcesses())
        {
            IntPtr hProcess = IntPtr.Zero;

            try
            {
                hProcess = ExternalDll.OpenProcess(ProcessActions.PROCESS_QUERY_INFORMATION | ProcessActions.PROCESS_VM_READ, false, process.Id);

                if (hProcess == IntPtr.Zero)
                {
                    continue;
                }

                StringBuilder buffer = new(1024);
                int size = buffer.Capacity;

                if (ExternalDll.QueryFullProcessImageName(hProcess, 0, buffer, ref size))
                {
                    string processPath = buffer.ToString().ToLowerInvariant();
                    if (processPath == exePath)
                    {
                        return Response<bool>.SetResponse(true, true, null, exceptionId);
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionId = GenerateGuid.New();

                _logger.LogWarning(message: ErrorMessages.UnableQueryProcess + " {processName} {Exception} {LogId}", [process.ProcessName, ex, exceptionId]);
            }
            finally
            {
                if (hProcess != IntPtr.Zero)
                {
                    ExternalDll.CloseHandle(hProcess);
                }
            }
        }

        return Response<bool>.SetResponse(false, true, null, exceptionId);
    }

    public Response<bool> OpenHandlerToProcess(int processId)
    {
        try
        {
            IntPtr openedProcessIdPtr = ExternalDll.OpenProcess(ProcessActions.PROCESS_ALL_ACCESS, false, processId);

            if (openedProcessIdPtr == IntPtr.Zero)
            {
                return Response<bool>.SetResponse(false, true, "Process is not running", null);
            }

            _processState.SetProcessHandleProcessIdPtr(openedProcessIdPtr);
            _processState.SetProcessId(processId);

            return Response<bool>.SetResponse(true, true, "Succsessfully attached to the running process", null);
        }
        catch (Exception ex)
        {
            string exceptionId = GenerateGuid.New();

            _logger.LogError(message: ErrorMessages.GenericExceptionMessage, [ex, exceptionId]);

            return Response<bool>.SetResponse(false, true, null, exceptionId);
        }
    }

    public Response<IEnumerable<ProcessInfo>> GetRunningApplications()
    {
        try
        {
            List<ProcessInfo> runningApps = [];

            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    if (process.HasExited)
                    {
                        continue;
                    }

                    if (process.MainWindowHandle != IntPtr.Zero)
                    {
                        runningApps.Add(new ProcessInfo
                        {
                            Name = process.ProcessName,
                            Id = process.Id,
                            Icon = GetProcessIcon(process).Value,
                            MemoryUsage = (process.WorkingSet64 / 1024 / 1024).ToString(),
                            CpuTime = process.TotalProcessorTime.ToString("g"),
                            StartTime = process.StartTime.ToString("g"),
                            ThreadsCount = process.Threads.Count.ToString(),
                        });
                    }
                }
                catch { /* Ignored access denied */ }
            }

            return Response<IEnumerable<ProcessInfo>>.SetResponse(runningApps, true, null, null);
        }
        catch (Exception ex)
        {
            string exceptionId = GenerateGuid.New();

            _logger.LogError(message: ErrorMessages.GenericExceptionMessage, [ex, exceptionId]);

            return Response<IEnumerable<ProcessInfo>>.SetResponse(null, true, null, exceptionId);
        }
    }

    private static IEnumerable<ProcessInfo> OrderByIcon(IEnumerable<ProcessInfo> processInfos)
    {
        return [.. processInfos.OrderByDescending(x => x.Icon is not null)];
    }
}
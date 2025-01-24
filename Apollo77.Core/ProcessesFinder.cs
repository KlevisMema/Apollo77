using Apollo77.Core;
using Apollo77.Shared;
using Apollo77.Shared.Util;

using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Linq;

namespace Apollo77.CORE;

public class ProcessesFinder(ILogger<ProcessesFinder> _logger) : IProcessesFinder
{
    public Response<List<ProcessInfo>> GetAllProcesses()
    {
        List<ProcessInfo> processList = [];

        foreach (Process process in Process.GetProcesses())
        {
            try
            {
                processList.Add(new ProcessInfo
                {
                    Name = process.ProcessName,
                    Id = process.Id,
                    Icon = GetProcessIcon(process).Value,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ErrorMessages.GenericExceptionMessage, ex);

                return Response<List<ProcessInfo>>.SetResponse(processList, false, ErrorMessages.GetProcessess);
            }
        }

        processList = [.. processList.OrderByDescending(pr => pr.Icon is not null)];

        return Response<List<ProcessInfo>>.SetResponse(processList, true, null);
    }

    public Response<BitmapImage?> GetProcessIcon(Process process)
    {
        try
        {
            string? filePath = process.MainModule?.FileName;

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(process), ErrorMessages.NullProccessFile);
            }

            Icon? icon = Icon.ExtractAssociatedIcon(filePath);

            if (icon == null)
            {
                return Response<BitmapImage?>.SetResponse(null, true, ErrorMessages.ProcessHasNoIcon);
            }

            using MemoryStream memory = new();
            icon.ToBitmap().Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;

            BitmapImage bitmapImage = new();
            bitmapImage.SetSource(memory.AsRandomAccessStream());

            return Response<BitmapImage?>.SetResponse(bitmapImage, true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(message: ErrorMessages.GenericExceptionMessage, ex);

            return Response<BitmapImage?>.SetResponse(null, false, null);
        }
    }
}
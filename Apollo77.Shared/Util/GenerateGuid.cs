using System;

namespace Apollo77.Shared.Util;

public static class GenerateGuid
{
    public static string New()
    {
        return Guid.NewGuid().ToString();
    }
}
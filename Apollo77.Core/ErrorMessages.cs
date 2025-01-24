namespace Apollo77.Core;

internal static class ErrorMessages
{
    private const string _exceptionMessageTemplaeParamName = "{exception}";
    internal const string GenericExceptionMessage = "Something went wrong: " + _exceptionMessageTemplaeParamName;
    internal const string GetProcessess = "Someting went wrong when getting running processess.";
    internal const string NullProccessFile = "Process file path is null.";
    internal const string ProcessHasNoIcon = "Process has no icon.";


}
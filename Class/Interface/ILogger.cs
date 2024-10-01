using ConfigurateService.Class.Enums;

namespace ConfigurateService.Class.Interface
{
    interface ILogger
    {
        void WtiteLog(in string message, StatusLog status=StatusLog.ACTION);
    }
}

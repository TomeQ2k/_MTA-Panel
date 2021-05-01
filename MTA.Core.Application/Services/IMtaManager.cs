using MTA.Core.Application.Services.Sdks;
using MTA.Core.Application.Settings;

namespace MTA.Core.Application.Services
{
    public interface IMtaManager
    {

        MtaServerSettings MtaServerSettings { get; }
        string CallFunction(string resourceName, string functionName, MtaLuaArgs args = null);

        MtaLuaArgs CreateParams(params object[] args);
    }
}
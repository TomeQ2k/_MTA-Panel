using Microsoft.Extensions.Options;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.Sdks;
using MTA.Core.Application.Settings;

namespace MTA.Infrastructure.Shared.Services
{
    public class MtaManager : IMtaManager
    {
        private readonly MtaServer mtaServer;

        public MtaServerSettings MtaServerSettings { get; }

        public MtaManager(IOptions<MtaServerSettings> mtaServerOptions)
        {
            MtaServerSettings = mtaServerOptions.Value;

            mtaServer = new MtaServer(MtaServerSettings.Host, MtaServerSettings.Port, MtaServerSettings.Username,
                MtaServerSettings.Password);
        }

        public string CallFunction(string resourceName, string functionName, MtaLuaArgs args = null)
            => mtaServer.CallFunction(resourceName, functionName, args ?? CreateParams());

        public MtaLuaArgs CreateParams(params object[] args) => new MtaLuaArgs(args);
    }
}
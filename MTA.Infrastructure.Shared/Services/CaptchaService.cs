using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.Settings;

namespace MTA.Infrastructure.Shared.Services
{
    public class CaptchaService : ICaptchaService
    {
        private readonly IHttpClientFactory httpClientFactory;

        private readonly CaptchaSettings captchaSettings;

        public CaptchaService(IHttpClientFactory httpClientFactory, IOptions<CaptchaSettings> captchaSettings)
        {
            this.httpClientFactory = httpClientFactory;

            this.captchaSettings = captchaSettings.Value;
        }

        public async Task<bool> VerifyCaptcha(string captchaResponse)
        {
            var httpClient = httpClientFactory.CreateClient();

            var response = await httpClient.PostAsync($"{captchaSettings.ApiUrl}siteverify",
                new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"secret", captchaSettings.SecretKey},
                    {"response", captchaResponse}
                }));

            response.EnsureSuccessStatusCode();

            var result =
                (await response.Content.ReadAsStringAsync()).FromJSON<VerifyCaptchaResult>(
                    options: JsonSettings.JsonSerializerOptions);

            return response.IsSuccessStatusCode ? result.Success : throw new CaptchaException();
        }
    }
}
namespace Lockec.Services.Sms
{
    using Lockec.Services.Api_rest;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    public class SmsService
    {

        private static readonly string USER = "seproamerica";
        private static readonly string PASS = "Ju@n_Ca45#s";
        private static readonly string RUTA = "S";
        private static readonly string CAMP = "Codigo de Verificacion";

        public async Task<Response> SendSms(string number, string code)
        {
            Response resp = null;
            using (var client = CreateClient())
            {
                var dict = new Dictionary<string, string>
                {
                    { "user", USER },
                    { "pass", PASS },
                    { "msg", "Estimado cliente, su codigo de verificacion LOCKEC es:" + code },
                    { "num", "0"+number },
                    { "ruta", RUTA },
                    { "camp", CAMP }
                };
                var req = new HttpRequestMessage(HttpMethod.Post, "https://app.massend.com/api/sms/")
                {
                    Content = new FormUrlEncodedContent(dict)
                };
                var response = await client.SendAsync(req).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    resp.IsSuccess = false;
                    resp.Message = response.StatusCode.ToString();
                }
            }
            return resp;
        }

        private const string ApiBaseAddress = "https://app.massend.com/api/sms/";
        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseAddress)
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

    }
}

namespace Lockec.Services.Api_rest
{
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class ApiService
    {
        public Response CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Por favor, conectate a internet",
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok",
            };
        }
        public async Task<Response> GetAllInfo<T>(string servicePrefix, string filter="")
        {
            using (var client = CreateClient())
            {
                var url = string.Format("{0}/{1}", servicePrefix, filter);
                Console.WriteLine("EN GET ALL INFO PARA VERIFICAR EL QUERY PARAM");
                Console.WriteLine(url);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = model,
                };
            }
        }

        public async Task<Response> GetWithParam<T>(string servicePrefix, string key, string value)
        {
            using (var client = CreateClient())
            {
                var url = string.Format("{0}/?{1}={2}", servicePrefix, key, value);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = model,
                };
            }
        }

        public async Task<string> GetAllProvided(string servicePrefix, string key, string value)
        {
            using (var client = CreateClient())
            {
                var url = string.Format("{0}/?{1}={2}", servicePrefix, key, value);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }


        public async Task<Response> Get<T>(string servicePrefix, string id)
        {
            using (var client = CreateClient())
            {
                var url = string.Format("{0}/{1}/", servicePrefix, id);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<T>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = model,
                };
            }
        }

        public async Task<Response> Post<T>(string servicePrefix, T model)
        {
            using (var client = CreateClient())
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var url = string.Format("{0}/", servicePrefix);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
                };
            }
            
        }


        public async Task<Response> Put<T>(string servicePrefix, T model, string id)
        {
            using (var client = CreateClient())
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var url = string.Format("{0}/{1}/", servicePrefix, id);
                var response = await client.PutAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = newRecord,
                };
            }
        }


        //AQUI SE CAMBIARIA A LA IP DEL HOST DEL API REST
        private const string ApiBaseAddress = "https://lockec.pythonanywhere.com/api/";
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

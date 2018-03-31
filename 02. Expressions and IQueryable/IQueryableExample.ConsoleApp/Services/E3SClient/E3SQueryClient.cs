using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using IQueryableExample.ConsoleApp.Services.E3SClient.Entities;
using Newtonsoft.Json;

namespace IQueryableExample.ConsoleApp.Services.E3SClient
{
    public class E3SQueryClient
    {
        private readonly string _userName;
        private readonly string _password;
        private readonly Uri _baseAddress;

        public E3SQueryClient(string user, string password, string url)
        {
            _userName = user;
            _password = password;
            _baseAddress = new Uri(url);
        }

        public IEnumerable<T> SearchFTS<T>(string query, int start = 0, int limit = 0) where T : E3SEntity
        {
            HttpClient client = CreateClient();
            var requestGenerator = new FTSRequestGenerator(_baseAddress);

            Uri request = requestGenerator.GenerateRequestUrl<T>(query, start, limit);

            var resultString = client.GetStringAsync(request).Result;

            return JsonConvert.DeserializeObject<FTSResponse<T>>(resultString).items.Select(t => t.data);
        }

        public IEnumerable SearchFTS(Type type, string query, int start = 0, int limit = 0)
        {
            HttpClient client = CreateClient();
            var requestGenerator = new FTSRequestGenerator(_baseAddress);
            Uri request = requestGenerator.GenerateRequestUrl(type, query, start, limit);

            var resultString = client.GetStringAsync(request).Result;
            var endType = typeof(FTSResponse<>).MakeGenericType(type);
            var result = JsonConvert.DeserializeObject(resultString, endType);

            var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(type)) as IList;

            foreach (object item in (IEnumerable)endType.GetProperty("items").GetValue(result))
            {
                list.Add(item.GetType().GetProperty("data").GetValue(item));
            }

            return list;
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = true,
                PreAuthenticate = true
            });

            var encoding = new ASCIIEncoding();
            var authHeader = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(encoding.GetBytes(string.Format("{0}:{1}", _userName, _password))));
            client.DefaultRequestHeaders.Authorization = authHeader;

            return client;
        }
    }
}

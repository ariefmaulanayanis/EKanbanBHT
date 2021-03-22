using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EKanbanBHT.Models
{
    public class KanbanManager
    {
        static readonly string BaseAddress = "https://192.168.100.18:45455";
        static readonly string Url = $"{BaseAddress}/api/kanban/";
        private string authorizationKey;

        private async Task<HttpClient> GetClient()
        {
            HttpClient client = new HttpClient(new HttpClientHandler());
            if (string.IsNullOrEmpty(authorizationKey))
            {
                authorizationKey = await client.GetStringAsync(Url + "login");
                authorizationKey = JsonConvert.DeserializeObject<string>(authorizationKey);
            }

            client.DefaultRequestHeaders.Add("Authorization", authorizationKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<List<KanbanItem>> GetAll()
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<List<KanbanItem>>(result);
        }
    }
}

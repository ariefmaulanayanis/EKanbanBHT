using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EKanbanBHT.Models
{
    public class KanbanManager
    {
        //static readonly string BaseAddress = "https://192.168.100.18:45455";
        //static readonly string Url = $"{BaseAddress}/api/kanban/";
        static string BaseAddress;
        static string Url;
        private string authorizationKey;

        public string StatusMessage { get; set; }
        public bool IsError { get; set; }

        public KanbanManager()
        {
            BaseAddress = Preferences.Get("api", "");
            Url = BaseAddress + "/api/kanban/";
        }

        private async Task<HttpClient> GetClient()
        {
            //HttpClient client = new HttpClient(new HttpClientHandler());
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; //add this line to run on android
            HttpClient client = new HttpClient(handler);
            if (string.IsNullOrEmpty(authorizationKey))
            {
                try
                {
                    authorizationKey = await client.GetStringAsync(Url + "login");
                    authorizationKey = JsonConvert.DeserializeObject<string>(authorizationKey);
                }
                catch(HttpRequestException e)
                {
                    StatusMessage = string.Format("HttpRequest Exception\n" + e.Message);
                    IsError = true;
                }
                catch(TaskCanceledException e)
                {
                    StatusMessage = string.Format("Timeout Exception\n"+e.Message);
                    IsError = true;
                }
                catch (Exception e)
                {
                    IsError = true;
                    StatusMessage = e.Message;
                }
            }

            client.DefaultRequestHeaders.Add("Authorization", authorizationKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<List<KanbanSync>> GetAll()
        {
            IsError = false;
            StatusMessage = "";
            string result = ""; ;
            try
            {
                HttpClient client = await GetClient();
                //if(!IsError) result = await client.GetStringAsync(Url);
                result = await client.GetStringAsync(Url);
            }
            catch (HttpRequestException e)
            {
                StatusMessage = string.Format("HttpRequest Exception\n" + e.Message);
                IsError = true;
            }
            catch (TaskCanceledException e)
            {
                StatusMessage = string.Format("Timeout Exception\n" + e.Message);
                IsError = true;
            }
            catch (Exception e)
            {
                IsError = true;
                StatusMessage = e.Message;
            }
            return JsonConvert.DeserializeObject<List<KanbanSync>>(result);
        }

        public async Task Update(KanbanHeader header)
        {
            try
            {
                HttpClient client = await GetClient();
                if (!IsError) 
                    await client.PutAsync(Url + header.KanbanReqId.ToString(),
                        new StringContent(
                            JsonConvert.SerializeObject(header),
                            Encoding.UTF8, "application/json"));
            }
            catch (HttpRequestException e)
            {
                StatusMessage = string.Format("HttpRequest Exception\n" + e.Message);
                IsError = true;
            }
            catch (TaskCanceledException e)
            {
                StatusMessage = string.Format("Timeout Exception\n" + e.Message);
                IsError = true;
            }
            catch (Exception e)
            {
                IsError = true;
                StatusMessage = e.Message;
            }
            //await client.PutAsync(Url + "/" + header.KanbanReqId,
            //    new StringContent(
            //        JsonConvert.SerializeObject(header),
            //        Encoding.UTF8, "application/json"));
        }
    }
}

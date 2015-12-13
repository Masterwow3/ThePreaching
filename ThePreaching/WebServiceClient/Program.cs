using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebServiceClient.Base.WebService;
using Entitie;
using System.Runtime.Serialization.Json;
using Entitie.Language;

namespace WebServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }
        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //// HTTP GET
                //HttpResponseMessage response = await client.GetAsync("api/products/1");
                //if (response.IsSuccessStatusCode)
                //{
                //    Product product = await response.Content.ReadAsAsync<Product>();
                //    Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                //}

                // HTTP POST
                var gizmo = new Request<LanguageEnum>() { Data = LanguageEnum.Afrikaans, UserGuid = "132"};

                string postBody = JsonSerializer(gizmo);
                HttpResponseMessage response = await client.PostAsync("ThePreaching/GetUserDefaultLanguage", new StringContent(postBody, Encoding.UTF8, "application/json"));
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                var content = await response.Content.ReadAsStreamAsync();
                object resp = jsonSerializer.ReadObject(content);
                //if (response.IsSuccessStatusCode)
                //{
                //    Uri gizmoUrl = response.Headers.Location;

                //    // HTTP PUT
                //    gizmo.Price = 80;   // Update price
                //    response = await client.PutAsJsonAsync(gizmoUrl, gizmo);

                //    // HTTP DELETE
                //    response = await client.DeleteAsync(gizmoUrl);
                //}
            }
        }

        //public static object JsonDeserializer(HttpResponseMessage response)
        //{
        //    new DataContractJsonSerializer()
        //}

        public static string JsonSerializer(object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                throw new ArgumentException("objectToSerialize must not be null");
            }
            MemoryStream ms = null;

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectToSerialize.GetType());
            ms = new MemoryStream();
            serializer.WriteObject(ms, objectToSerialize);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            return sr.ReadToEnd();
        }
    }
}

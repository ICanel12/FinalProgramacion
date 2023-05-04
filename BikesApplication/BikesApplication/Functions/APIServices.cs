using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BikesApplication.Functions
{
    public class APIServices
    {

        private static int timeout = 30;
        private static string baseurl = "https://localhost:7082/";


        public static async System.Threading.Tasks.Task<BikesApplicationModel.Token> LoginAPILogin(BikesApplicationModel.Token object_to_serialize)
        {
            var json_ = Newtonsoft.Json.JsonConvert.SerializeObject(object_to_serialize);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);

            var response = await httpClient.PostAsync(baseurl + "Login/Login", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<BikesApplicationModel.Token>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }

        }



        public static async System.Threading.Tasks.Task<IEnumerable<BikesApplicationModel.Bike>> GetBikes(string accessToken)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.GetAsync(baseurl + "Bikes/GetBikes");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<BikesApplicationModel.Bike>>(await response.Content.ReadAsStringAsync());
            }

            else
            {
                throw new Exception(response.StatusCode.ToString());

            }
        }


        public static async System.Threading.Tasks.Task BikeSet(BikesApplicationModel.Bike object_to_serialize, string accessToken)
        {
            var json_ = Newtonsoft.Json.JsonConvert.SerializeObject(object_to_serialize);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };


            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);

            var response = await httpClient.PostAsync(baseurl + "Bikes/CreateBike", content);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

    }
}

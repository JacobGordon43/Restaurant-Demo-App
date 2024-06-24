using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using Restaurant_Demo_App.Models;
using System.Diagnostics;

namespace Restaurant_Demo_App.Services
{
    public class AdminDAO
    {
        private string API = "https://f9mv86hy5b.execute-api.us-west-1.amazonaws.com/Prod";
        private HttpClient client = new HttpClient();
        public AdminDAO() { }

        public async Task<bool> CreateAdmin(AdminModel admin)
        {
            bool created = false;
            JsonContent adminJson = JsonContent.Create(admin);
            Debug.WriteLine(adminJson.Value);
            var results = await client.PostAsync(API + "/login", adminJson);

            Debug.WriteLine(await results.Content.ReadAsStringAsync());
            if(results.StatusCode == System.Net.HttpStatusCode.OK)
            {
                created = true;
            }

            return created;
        }

        public async Task<bool> LoginAdmin(AdminModel admin)
        {
            bool loggedIn = false;
            JsonContent adminJson = JsonContent.Create(admin);

            var results = await client.PostAsync(API + "/login", adminJson);
            if (results.StatusCode == System.Net.HttpStatusCode.OK)
            {
                loggedIn = true;
            }

            return loggedIn;
        }
    }
}

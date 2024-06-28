using Firebase.Auth;
using FirebaseAdmin.Auth;
using Google.Apis.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using Refit;
using Restaurant_Demo_App.Models;
using System.Diagnostics;

namespace Restaurant_Demo_App.Services
{
    public class AdminDAO
    {
        private const string API_KEY = "AIzaSyDQT_OI6CQKI99q2raph8nKbwG8Mx8uh84";
        private HttpClient client = new HttpClient();
        public AdminDAO() { }

        FirebaseAuthProvider firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(API_KEY));

  /*      public async Task<bool> CreateAdmin(AdminModel admin)
        {
            bool created = false;
            try
            {
                FirebaseAuthLink firebaseAuthLink = await firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(admin.Email, admin.Password, admin.Name, false);
                Debug.WriteLine(firebaseAuthLink.FirebaseToken);
                if(firebaseAuthLink.FirebaseToken != null || firebaseAuthLink.FirebaseToken != "") 
                {
                    created = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return created;
        }
*/
        public async Task<bool> LoginAdmin(AdminModel admin)
        {
            bool success = false;
            try
            {
                FirebaseAuthLink firebaseAuthLink = await firebaseAuthProvider.SignInWithEmailAndPasswordAsync(admin.Email, admin.Password);

                FirebaseToken decoded = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(firebaseAuthLink.FirebaseToken);
                object Id;
                decoded.Claims.TryGetValue("user_id", out Id);
                Debug.WriteLine(Id);
                IDataService dataService = RestService.For<IDataService>("https://localhost:7019/api/GetData");

                await dataService.GetData(firebaseAuthLink.FirebaseToken);
                if (firebaseAuthLink.FirebaseToken != null || firebaseAuthLink.FirebaseToken != "")
                {
                    success = true;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return success;
        }
    }
}

public interface IDataService
{
    [Get("/")]
    Task GetData([Authorize("Bearer")] string token);
}
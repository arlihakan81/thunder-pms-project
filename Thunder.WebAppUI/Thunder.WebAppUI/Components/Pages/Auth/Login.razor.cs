using Microsoft.AspNetCore.Components;
using Thunder.Application.Models;

namespace Thunder.WebAppUI.Components.Pages.Auth
{
    public partial class Login
    {
        [Inject]
        public required LoginModel Model { get; set; } 
        [Inject]
        public required HttpClient Client { get; set; } 
        [Inject]
        public required NavigationManager NavigationManager { get; set; }


        public async Task Handle(LoginModel model)
        {
            Client.BaseAddress = new Uri("https://localhost:7001/api/");
            var res = await Client.PostAsJsonAsync<LoginModel>(Client.BaseAddress+"Auth/Login", model);
            if(res != null && res.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                Console.WriteLine("An unhandled error occured");
            }
        }


    }
}

using Restaurant_Demo_App.Models;

namespace Restaurant_Demo_App.Services
{
    public class AdminService
    {
        private AdminDAO adminDAO = new AdminDAO();
        public AdminService() { }

/*        public async Task<bool> CreateAdmin(AdminModel admin) 
        {
            bool created = false;
            string id = System.Guid.NewGuid().ToString();
            admin.Id = id;
            if (await adminDAO.CreateAdmin(admin)){
                created = true;
            }
            
            return created;
        }*/

        public async Task<bool> LoginAdmin(AdminModel admin)
        {
            bool loggedIn = false;
            if (await adminDAO.LoginAdmin(admin))
            {
/*                httpContext.Session.SetString("Name", verifiedAdmin.Name);
                httpContext.Session.SetString("Id", verifiedAdmin.Id);*/
                loggedIn = true;
            }

            return loggedIn;
        }
    }

}

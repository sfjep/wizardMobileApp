using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
//using XamarinApp.Model;

namespace WizardScoreboard
{
    class FirebaseHelper
    {
        private readonly string ChildName = "Users";

        readonly FirebaseClient firebase = new FirebaseClient("https://wizardapp-b2626.firebaseio.com/");

        public async Task<List<Users>> GetAllUsers()
        {
            return (await firebase
                .Child(ChildName)
                .OnceAsync<Users>()).Select(item => new Users()
                {
                    Username = item.Object.Username,
                    Password = item.Object.Password
                }).ToList();
        }

        public async Task AddUser(string username, string password)
        {
            await firebase
                .Child(ChildName)
                .PostAsync(new Users() { Username = username, Password = password});
        }

        public async Task<Users> GetUser(string username)
        {
            var allUsers = await GetAllUsers();
            await firebase
                .Child(ChildName)
                .OnceAsync<Users>();
            return allUsers.FirstOrDefault(a => a.Username == username);
        }

        public async Task<Users> GetUsers(string username)
        {
            var allUsers = await GetAllUsers();
            await firebase
                .Child(ChildName)
                .OnceAsync<Users>();
            return allUsers.FirstOrDefault(a => a.Username == username);
        }

        public async Task UpdateUser(string username)
        {
            var toUpdateUser = (await firebase
                .Child(ChildName)
                .OnceAsync<Users>()).FirstOrDefault(a => a.Object.Username == username);

            await firebase
                .Child(ChildName)
                .Child(toUpdateUser.Key)
                .PutAsync(new Users() { Username = username});
        }

        public async Task DeleteUser(string username)
        {
            var toDeleteUser = (await firebase
                .Child(ChildName)
                .OnceAsync<Users>()).FirstOrDefault(a => a.Object.Username == username);
            await firebase.Child(ChildName).Child(toDeleteUser.Key).DeleteAsync();
        }
    }
}

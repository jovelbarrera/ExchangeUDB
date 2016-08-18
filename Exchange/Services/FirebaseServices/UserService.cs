using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.Helpers;
using Exchange.Models;
using Exchange.Services.FirebaseServices;

namespace Exchange.Services.FirebaseServices
{
    public class UserService : FirebaseDatabaseService<UserService, User>
    {
        protected override string Resource { get { return "User"; } }

        protected override string Token { get { return Settings.FirebaseUserToken; } }

        protected override string BaseUrl { get { return Configs.FirebaseAccess.Instance.FirebaseBasePath; } }

        protected override Dictionary<string, string> Headers { get { return new Dictionary<string, string>(); } }

        public async Task Register(User user)
        {
            DateTime currentTime = await TimeService.Instance.Now();
            try
            {
                var registeredUser = await ReadSingle(user.ObjectId);
                user.UpdatedAt = currentTime;
                if (registeredUser == null)
                {
                    user.CreatedAt = currentTime;
                    await base.Create(user);
                }
                else
                {
                    user.CreatedAt = registeredUser.CreatedAt;
                    await base.Update(user);
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.Message == "401 (Unauthorized)")
                    await App.Current.MainPage.DisplayAlert("Error", "No tienes permisos para completar esta acción", "ACEPTAR");
                if (ex.Message == "400 (Bad Request)")
                    await App.Current.MainPage.DisplayAlert("Error", "Un error ocurrió al intentar realizar esta acción", "ACEPTAR");
                else
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ACEPTAR");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ACEPTAR");
            }
        }

        public async Task<User> Get(string objectId)
        {
            return await ReadSingle(objectId);
        }
    }
}


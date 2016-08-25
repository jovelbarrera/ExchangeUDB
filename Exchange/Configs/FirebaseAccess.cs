using System;
using Exchange.Services.FirebaseServices;

namespace Exchange.Configs
{
    class FirebaseAccess : IFirebaseAccess
    {
        private static FirebaseAccess _instance;
        public static FirebaseAccess Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FirebaseAccess();
                return _instance;
            }
        }

        public string ApiKey
        {
            get
            {
                return "AIzaSyDm0IwNgOF2s3wpYjPDkAbrDN1mGDfNgww";
            }
        }

        public string FirebaseBasePath
        {
            get
            {
                return "https://exchange-fcbe1.firebaseio.com/";
            }
        }
    }
}

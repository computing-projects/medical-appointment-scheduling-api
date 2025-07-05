using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace medical_appointment_scheduling_api.Services
{
    public class FirebaseTokenService
    {
        public FirebaseTokenService()
        {
            if (FirebaseApp.DefaultInstance == null) // Cria apenas se não estiver criado ainda
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile("Config/firebase-service-account.json")
                });
            }
        }


        public async Task<FirebaseToken> VerifyTokenAsync(string idToken)
        {
            try
            {

                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                return decodedToken;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Invalid Firebase ID token.", ex);
            }
        }
    }
}

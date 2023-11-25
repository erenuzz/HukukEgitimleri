using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using NuGet.Protocol;
using System.Text;
using Google.Apis.Util;

namespace HukukSinavlari.Areas.Control.Models
{
    public class GoogleCalendarManager
    {
        static string[] Scopes = { CalendarService.Scope.CalendarEvents };
        static string ApplicationName = "HukukEgitimleri";

        public GoogleCalendarManager()
        {
        }

        public string AddEvent(Event newEvent)
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".credentials/token.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Specify the calendarId
            string calendarId = "primary";  // "primary" kullanıcı hesabının varsayılan takvimini temsil eder.

            // Etkinlik oluştur
            var request = service.Events.Insert(newEvent, calendarId);
            var createdEvent = request.Execute();

            // Oluşturulan etkinlikten alınan linki geri döndür
            return createdEvent.HtmlLink;
        }


    }
}

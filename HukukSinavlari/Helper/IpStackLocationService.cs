using HukukSinavlari.Models;
using System.Text.Json;

namespace HukukSinavlari.Helper
{
    public class IpStackLocationService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public IpStackLocationService(string apiKey = "6495ec7f1cb1adbd2a816bda96ee3b8a")
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
        }

        public async Task<IpStackLocation> GetLocationFromIpAsync(string ipAddress)
        {
            var apiUrl = $"http://api.ipstack.com/{ipAddress}?access_key={_apiKey}";

            // İstek gönderme ve yanıtı alıp geri döndürme
            var response = await _httpClient.GetStringAsync(apiUrl);

            // JSON'dan istediğiniz bilgileri çıkarabilirsiniz.
            var location = JsonSerializer.Deserialize<IpStackLocation>(response);

            return location;
        }
    }
}

namespace sansidalgo.Server
{
    public class NseApiService
    {
        private readonly HttpClient _httpClient;

        public NseApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetNseDataAsync(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return json;
            }

            return null;
        }
    }
}

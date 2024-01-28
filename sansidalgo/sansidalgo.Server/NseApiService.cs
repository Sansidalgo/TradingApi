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
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return json;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
           

            return null;
        }
    }
}

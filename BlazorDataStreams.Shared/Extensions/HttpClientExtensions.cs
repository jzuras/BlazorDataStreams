using System.Text.Json;

namespace BlazorDataStreams.Shared;

public static class HttpClientExtensions
{
    public static async Task<T> ReadJsonContentAsync<T>(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode == false)
            throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

        var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonSerializer.Deserialize<T>(
            dataAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }
}

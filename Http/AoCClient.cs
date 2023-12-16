using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Advent.Http;

public class AocClient
{
    private const string Cookie = "Cookie";
    private const string Session = "session=";
    private readonly HttpClient client;
    public AocClient(string token)
    {
        client = new HttpClient();
        if(token != string.Empty)
            client.DefaultRequestHeaders.Add(Cookie, $"{Session}{token}");
    }

    public async Task<string> FetchInputAsync(int year, int day)
    {
        if(!client.DefaultRequestHeaders.Any(h => h.Key == Cookie && h.Value.Contains($"{Session}")))
        {
            Console.WriteLine("The http client has an emtpy token. Either the environment variable 'AOC_TOKEN' does not exist or the client was created incorrectly.");
            return string.Empty;
        }
        try
        {
            var uri = $"https://adventofcode.com/{year}/day/{day}/input";
            var request = await client.GetAsync(uri);
            request.EnsureSuccessStatusCode();
            return await request.Content.ReadAsStringAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to download input for {year}/{day}: {e.Message}");
        }
        return string.Empty;
    }
}

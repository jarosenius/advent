using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Advent.Http;

public class AocClient
{
    private readonly HttpClient client;
    public AocClient(string token)
    {
        client = new HttpClient();
        client.DefaultRequestHeaders.Add("Cookie", $"session={token}");
    }

    public async Task<string> FetchInputAsync(int year, int day)
    {
        try
        {
            var uri = $"https://adventofcode.com/{year}/day/{day}/input";
            var request = await client.GetAsync(uri);
            request.EnsureSuccessStatusCode();
            return await request.Content.ReadAsStringAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}

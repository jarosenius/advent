using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Advent.Http;

public class AocClient
{
    private const string Cookie = "Cookie";
    private const string Session = "session=";
    private readonly HttpClient _client;
    public AocClient(IConfiguration configuration)
    {
        var token = configuration["AOC_TOKEN"] ?? throw new ArgumentException("AOC_TOKEN not found in environment variables or secrets.");
        _client = new HttpClient();
        if (token != string.Empty)
            _client.DefaultRequestHeaders.Add(Cookie, $"{Session}{token}");
    }

    public async Task<string> FetchInputAsync(int year, int day)
    {
        if (!_client.DefaultRequestHeaders.Any(h => h.Key == Cookie && h.Value.First().Contains($"{Session}")))
        {
            Console.WriteLine("The http client has an emtpy token. Either the environment variable 'AOC_TOKEN' does not exist or the client was created incorrectly.");
            return string.Empty;
        }
        try
        {
            var uri = $"https://adventofcode.com/{year}/day/{day}/input";
            var request = await _client.GetAsync(uri);
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

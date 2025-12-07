using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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

    public async Task<string> FetchExampleInputAsync(int year, int day)
    {
        try
        {
            var uri = $"https://adventofcode.com/{year}/day/{day}";
            var response = await _client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            return ExtractExampleInput(html);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to download example input for {year}/{day}: {e.Message}");
        }
        return string.Empty;
    }

    private static string ExtractExampleInput(string html)
    {
        const string pattern = "For example:.*?<pre><code>(.*?)</code></pre>";
        var match = Regex.Match(html, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

        if (match.Success)
        {
            var content = match.Groups[1].Value;
            return DecodeHtml(content);
        }

        var allCodeBlocks = Regex.Matches(html, "<pre><code>(.*?)</code></pre>", RegexOptions.Singleline);

        foreach (Match codeBlock in allCodeBlocks)
        {
            var content = codeBlock.Groups[1].Value;
            var decoded = DecodeHtml(content).Trim();

            if (decoded.Contains('\n') && !content.Contains("<em>"))
            {
                return decoded;
            }
        }

        return string.Empty;
    }

    private static string DecodeHtml(string html)
    {
        return html
            .Replace("&lt;", "<")
            .Replace("&gt;", ">")
            .Replace("&amp;", "&")
            .Replace("&quot;", "\"")
            .Replace("&#39;", "'");
    }
}

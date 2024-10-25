using Newtonsoft.Json;
using SynergisticLogging.Web.Data;
using SynergisticLogging.Web.External.RapidApi;

namespace SynergisticLogging.Web.External;

public interface IDadJokesApiService
{
    Task<DadJoke> GetRandom();
}
public class DadJokesApiService : IDadJokesApiService
{
    private readonly IRapidApiClient _rapidApiClient;

    public DadJokesApiService(IRapidApiClient rapidApiClient)
    {
        _rapidApiClient = rapidApiClient;
    }
        
    public async Task<DadJoke> GetRandom()
    {
        var response = await _rapidApiClient.Get("https://dad-jokes.p.rapidapi.com/random/joke", Guid.NewGuid());

        if (!response.IsSuccessStatusCode) 
            throw new Exception("I just didn't get the joke this time");
        
        var jokeResponse = JsonConvert.DeserializeObject<RapidApiDadJokeResponse>(response.ResponseBody);

        var joke = new DadJoke
        {
            Setup = jokeResponse.body.First().setup,
            Punchline = jokeResponse.body.First().punchline
        };
        return joke;
    }
}
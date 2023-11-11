using MatchOdds.Models;
using MatchOdds.Resources;
using MatchOdds.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MatchOdds.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchOddsController : ControllerBase
    {
        private static IDBService _dBService;

        public MatchOddsController(IDBService dBService)
        {
            _dBService = dBService;
        }

        // GET: api/Match
        [HttpGet("/Match")]
        public async Task<MatchResponse> GetMatch(int id)
        {
            var response = new MatchResponse();
            try
            {
                var match = await _dBService.GetMatchWithId(id);
                response.Matches.Add(match);
                response.Status = Messages.RESPONSE_STATUS_OK;
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet("/MatchList")]
        public async Task<MatchResponse> GetMatchList(DateTime? date, string? team, Sport? sport)
        {
            var response = new MatchResponse();
            try
            {
                var matchList = await _dBService.GetMatchList(date, team, sport);
                response.Matches = matchList;
                response.Status = Messages.RESPONSE_STATUS_OK;
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpPost("/Match")]
        public async Task<MatchResponse> AddMatch(AddMatchRequest newMatch)
        {
            var response = new MatchResponse();

            try
            {
                var created = await _dBService.AddMatch(newMatch);
                response.Matches.Add(created);
                response.Status = Messages.RESPONSE_STATUS_OK;
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpPost("/MatchOdds")]
        public async Task<MatchOddsResponse> AddMatchOdds(AddMatchOddsRequest matchOddsRequest)
        {
            var response = new MatchOddsResponse();

            try
            {
                var created = await _dBService.AddMatchOdds(matchOddsRequest);
                response.MatchOdds.AddRange(created);
                response.Status = Messages.RESPONSE_STATUS_OK;
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

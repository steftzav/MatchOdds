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
        public async Task<ActionResult<MatchResponse>> GetMatch(int matchId)
        {
            var response = new MatchResponse();
            try
            {
                var match = await _dBService.GetMatchWithId(matchId);
                response.Matches.Add(match);
                response.Status = Messages.RESPONSE_STATUS_OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("/MatchOdd")]
        public async Task<ActionResult<MatchOddsResponse>> GetMatchOdd(int matchOddId)
        {
            var response = new MatchOddsResponse();
            try
            {
                var matchOdd = await _dBService.GetMatchOddWithId(matchOddId);
                response.MatchOdds.Add(new MatchOddItem()
                {
                    Id = matchOdd.Id,
                    MatchId = matchOdd.MatchId,
                    Specifier = matchOdd.Specifier,
                    Odd = matchOdd.Odd
                });
                response.Status = Messages.RESPONSE_STATUS_OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("/MatchList")]
        public async Task<ActionResult<MatchResponse>> GetMatchList(DateTime? date, string? team, Sport? sport)
        {
            var response = new MatchResponse();
            try
            {
                var matchList = await _dBService.GetMatchList(date, team, sport);
                response.Matches = matchList;
                response.Status = Messages.RESPONSE_STATUS_OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPost("/Match")]
        public async Task<ActionResult<MatchResponse>> AddMatch(AddMatchRequest newMatch)
        {
            var response = new MatchResponse();

            try
            {
                var created = await _dBService.AddMatch(newMatch);
                response.Matches.Add(created);
                response.Status = Messages.RESPONSE_STATUS_OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPost("/MatchOdds")]
        public async Task<ActionResult<MatchOddsResponse>> AddMatchOdds(AddMatchOddsRequest matchOddsRequest)
        {
            var response = new MatchOddsResponse();

            try
            {
                var created = await _dBService.AddMatchOdds(matchOddsRequest);
                response.MatchOdds.AddRange(created);
                response.Status = Messages.RESPONSE_STATUS_OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPut("/Match")]
        public async Task<ActionResult<MatchResponse>> UpdateMatch(int matchId, AddMatchRequest updateMatch)
        {
            var response = new MatchResponse();

            try
            {
                var updated = await _dBService.UpdateMatch(matchId, updateMatch);
                response.Matches.Add(updated);
                response.Status = Messages.RESPONSE_STATUS_OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPut("/MatchOdds")]
        public async Task<ActionResult<MatchOddsResponse>> UpdateMatchOdd(UpdateMatchOddRequest updateMatchOdd)
        {
            var response = new MatchOddsResponse();

            try
            {
                var updated = await _dBService.UpdateMatchOdd(updateMatchOdd);
                response.MatchOdds.Add(new MatchOddItem()
                {
                    Id = updated.Id,
                    MatchId = updated.MatchId,
                    Specifier = updated.Specifier,
                    Odd = updated.Odd
                });
                response.Status = Messages.RESPONSE_STATUS_OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpDelete("/Match")]
        public async Task<ActionResult<MatchResponse>> DeleteMatch(int matchId)
        {
            var response = new MatchResponse();
            try
            {
                response.Status = await _dBService.DeleteMatch(matchId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpDelete("/MatchOdds")]
        public async Task<ActionResult<MatchOddsResponse>> DeleteMatchOdd(int matchOddId)
        {
            var response = new MatchOddsResponse();
            try
            {
                response.Status = await _dBService.DeleteMatchOdd(matchOddId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = Messages.RESPONSE_STATUS_FAIL;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }
    }
}

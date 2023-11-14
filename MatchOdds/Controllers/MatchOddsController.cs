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

        /// <summary>
        /// Fetch a Match with its id.
        /// </summary>
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

        /// <summary>
        /// Fetch a MatchOdd with its id.
        /// </summary>
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

        /// <summary>
        /// Fetch a list of Matches using filters.
        /// </summary>
        /// <remarks>
        /// If a filter is null or empty it's ignored.
        /// Sport must be 1 or 2.
        /// Date must be in valid format.
        /// </remarks>
        [HttpGet("/MatchList")]
        public async Task<ActionResult<MatchResponse>> GetMatchList(string? date, string? team, Sport? sport)
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

        /// <summary>
        /// Create a Match.
        /// </summary>
        /// <remarks>
        /// Description is optional.
        /// Sport must be 1 or 2.
        /// MatchDateTime must be in valid format.
        /// </remarks>
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

        /// <summary>
        /// Create Odds for a Match.
        /// </summary>
        /// <remarks>
        /// matchId and id properties inside the matchOdds object are not used for this call.
        /// odd must be a float >= 1
        /// </remarks>
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

        /// <summary>
        /// Update a Match's info using its id.
        /// </summary>
        /// <remarks>
        /// Only updates non-empty fields of request.
        /// Sport must be 1 or 2.
        /// MatchDateTime must be in valid format.
        /// </remarks>
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

        /// <summary>
        /// Update a Match Odd's info using its id.
        /// </summary>
        /// <remarks>
        /// Only updates non-empty fields of request.
        /// odd must be a float >= 1
        /// </remarks>
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

        /// <summary>
        /// Delete a Match using its id.
        /// </summary>
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

        /// <summary>
        /// Delete a Match Odd using its id.
        /// </summary>
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

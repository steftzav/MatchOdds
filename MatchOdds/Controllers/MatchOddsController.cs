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
    }
}

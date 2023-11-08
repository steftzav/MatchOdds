using MatchOdds.Models;
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
    }
}

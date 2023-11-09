using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Metrics;
using MatchOdds.Models;
using MatchOdds.Resources;

namespace MatchOdds.Services
{
    public class DBService : IDBService
    {
        private readonly MatchOddsContext _context;

        public DBService(MatchOddsContext context)
        {
            _context = context;
        }

        public async Task<Match> GetMatchWithId(int? id)
        {
            if(!id.HasValue || id == 0)
            {
                throw new ArgumentNullException("id");
            }
            else if (!_context.Matches.Any(x => x.Id == id))
            {
                throw new KeyNotFoundException(Messages.MATCH_NOT_FOUND + id.ToString());
            }
            return await _context.Matches.Include(m => m.MatchOdds).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task<List<Match>> GetMatchList(DateTime? matchDate, Sport? sport)
        {
            return await _context.Matches.Include(m => m.MatchOdds).Where( m => 
                (matchDate == null || m.MatchDate == matchDate) &&
                (sport.HasValue || m.Sport == (int)sport)
            ).ToListAsync();
        }
    }
}
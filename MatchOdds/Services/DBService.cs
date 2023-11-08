using Microsoft.Extensions.Hosting;
using MatchOdds.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Metrics;

namespace MatchOdds.Services
{
    public class DBService : IDBService
    {
        private readonly MatchOddsContext _context;

        public DBService(MatchOddsContext context)
        {
            _context = context;
        }
    }
}
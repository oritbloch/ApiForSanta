using DBContextMigrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RequestsForSanta.Controllers
{
    [Route("api/santa")]
    public class SantaController : Controller
    {
        private readonly SantaDBContext _dbContext;
        public SantaController(SantaDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public List<FinalResponse> GetFinalList()
        {
            var result = from cgr in _dbContext.GiftRequests
                         join gc in _dbContext.GiftsOfChild on cgr.Id equals gc.requestID
                         group gc by cgr into g
                         select new FinalResponse
                         {
                             Address = g.Key.Address,
                             Gifts = g.OrderByDescending(gc => gc.gift.Price)
                                      .Select(gc => gc.gift.Name)
                                      .ToArray()
                         };

            return result.ToList();
        }
    }
}

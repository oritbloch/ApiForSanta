using DBContextMigrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RequestsForSanta.Controllers
{
    [Route("[controller]")]
    public class ChildrenController : ControllerBase
    {
        private readonly ILogger<ChildrenController> _logger;
        private readonly SantaDBContext _dbContext;
        private object _lock = new object();

        public ChildrenController(ILogger<ChildrenController> logger, SantaDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost(Name ="AddRequest")]
        public async Task<ActionResult<GiftRequest>> CreateRequest(GiftRequest giftRequest)
        {
            if (giftRequest.Gifts == null|| giftRequest.Gifts.Count==0)
            {
                return BadRequest("Gift Not Found");
            }
           
            try
            {
                lock (_lock)
                {
                    List<GiftOfChild> giftsList = new List<GiftOfChild>();
                    ChildGiftRequest req = ServiceHelper.GetRequestOfChild(_dbContext, giftRequest.ChildName, giftRequest.Age);
                    long requestId = -1;
                    if (req != null && req.Id > 0)
                    {
                        requestId = req.Id;
                    }
                    foreach (GiftDetails gd in giftRequest.Gifts)
                    {
                        GiftOfChild giftByName = ServiceHelper.GetSpecificGiftOfChild(_dbContext, giftRequest.ChildName, giftRequest.Age, gd.Name);

                        //check if there is already same gift in previous requests of child
                        if (giftByName != null && giftByName.gift.Id > 0)
                        {
                            return BadRequest("Item had already requested");
                        }

                        GiftOfChild gc = new GiftOfChild();
                        gc.color = gd.Color;
                        if (requestId > -1)
                        {
                            gc.requestID = requestId;
                        }
                        Gift existsingGift = ServiceHelper.GetGiftByName(_dbContext, gd.Name);

                        gc.gift = existsingGift;
                        giftsList.Add(gc);
                    }
                    if (requestId > 0)
                    {
                        giftsList.ForEach(x =>
                        {
                            req.childGifts.Add(x);
                        });
                        double totalPrice = req.childGifts.Sum(g => g.gift.Price);
                        if (totalPrice > 50)
                        {
                            return BadRequest("price for child is too much");
                        }
                        _dbContext.Entry(req).State = EntityState.Modified;
                    }
                    else
                    {
                        _dbContext.GiftRequests.Add(new ChildGiftRequest
                        {
                            Address = giftRequest.Address,
                            Age = giftRequest.Age,
                            ChildName = giftRequest.ChildName,
                            childGifts = giftsList
                        });
                    }

                    _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                //write a log somewhere
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut(Name = "UpdateRequest")]
        public async Task<IActionResult> UpdateRequest(string giftName, string name, string age, string? address, string? color)
        {
            if (String.IsNullOrEmpty(giftName))
            {
                return BadRequest("Gift Not Found");
            }
            lock (_lock)
            {
                Gift giftByName = ServiceHelper.GetGiftByName(_dbContext, giftName);
                if (giftByName != null)
                {
                    List<ChildGiftRequest> reqRows = _dbContext.GiftRequests.Where(x =>
                    x.ChildName == name &&
                    x.Age == Convert.ToDouble(age)).ToList();
                    if (reqRows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(address))
                        {

                            reqRows.ForEach(r =>
                            {
                                _dbContext.Entry(r).State = EntityState.Modified;
                                r.Address = address;
                            });
                        }
                        if (!string.IsNullOrEmpty(color))
                        {
                            GiftOfChild gift = _dbContext.GiftsOfChild.Where(x => x.gift.Name == giftName).FirstOrDefault();
                            _dbContext.Entry(gift).State = EntityState.Modified;
                            gift.color = color;
                        }
                    }

                    try
                    {
                         _dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        //write a log somewhere, handle the errors of constrains
                        throw;
                    }
                }
            }
            return Ok();
        }
    }
}

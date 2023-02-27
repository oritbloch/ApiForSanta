using DBContextMigrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RequestsForSanta;
using System.Net;
using System.Net.Http.Json;


namespace TestRequestsForSanta
{
    public class TestChildRequests
    {

        private IConfiguration _configuration;
        private SantaDBContext _dbContext;
        private RequestsForSanta.Controllers.ChildrenController childrenController;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();
            _dbContext = new SantaDBContext(_configuration);
            childrenController= new RequestsForSanta.Controllers.ChildrenController(null, _dbContext);
        }

        [Test]
        public async Task CreateRequest()
        {
            
            GiftRequest giftRequst = new GiftRequest()
            {
                Address = "New York",
                ChildName = "Elizabeth",
                Age = 5,
                Gifts = new List<GiftDetails>
                {
                    new GiftDetails()
                    {
                        Color = "Red",
                        Name = "Barbie"
                    }
                }
            };
            var result=await childrenController.CreateRequest(giftRequst);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public async Task CreateOver50CoinsRequest()
        {
            //checkLimit
            GiftRequest giftRequst2 = new GiftRequest()
            {
                Address = "New York",
                ChildName = "Elizabeth",
                Age = 5,
                Gifts = new List<GiftDetails>
                {
                    new GiftDetails()
                    {
                        Color = "Red",
                        Name = "PSP"
                    } 

                }
            };
            
            var result= await childrenController.CreateRequest(giftRequst2);
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task TestUpdateColor()
        {
            GiftRequest giftRequst = new GiftRequest()
            {
                Address = "New York",
                ChildName = "Elizabeth",
                Age = 5,
                Gifts = new List<GiftDetails>
                {
                    new GiftDetails()
                    {
                        Color = "Red",
                        Name = "Barbie"
                    }
                }
            };
            var result=await childrenController.UpdateRequest(giftRequst.Gifts[0].Name, giftRequst.ChildName, giftRequst.Age.ToString(), null, "Yellow");

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task TestUpdateAddress()
        {
            GiftRequest giftRequst = new GiftRequest()
            {
                Address = "New York",
                ChildName = "Elizabeth",
                Age = 5,
                Gifts = new List<GiftDetails>
                {
                    new GiftDetails()
                    {
                        Color = "Red",
                        Name = "Barbie"
                    }
                }
            };
           var result= await  childrenController.UpdateRequest(giftRequst.Gifts[0].Name, giftRequst.ChildName, giftRequst.Age.ToString(), "Tel Aviv", null);

            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}
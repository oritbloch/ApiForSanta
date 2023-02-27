using DBContextMigrations;
using Microsoft.Extensions.Configuration;
using RequestsForSanta;
using System.Net;
using System.Net.Http.Json;


namespace TestRequestsForSanta
{
    public class TestSantaList
    {

        private IConfiguration _configuration;
        private SantaDBContext _dbContext;
        private RequestsForSanta.Controllers.SantaController santaController;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();
            _dbContext = new SantaDBContext(_configuration);
            santaController = new RequestsForSanta.Controllers.SantaController(_dbContext);
        }

        [Test]
        public async Task TestFinalList()
        {

           List<FinalResponse> result= santaController.GetFinalList();

            Assert.Pass();
        }

    }
}
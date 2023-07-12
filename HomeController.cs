using ElasticSearchData.Entity;
using ElasticSearchData.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ElasticSearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        IElasticSearchService _elasticSearchService;
        public HomeController(IElasticSearchService elasticSearchService)
        {
            _elasticSearchService = elasticSearchService;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAsync()
        {
            await InsertFullData();
            return Ok();
        }

        private async Task InsertFullData()
        {
            List<Cities> cities = new List<Cities>()
            {
                new Cities {Id = Guid.NewGuid().ToString(),CreateDate= DateTime.Now, City ="Pune", Country = "india", Phone = "020-586462",PostalCode = "58568", Region = "Asia", Population = 485722 },
                new Cities {Id = Guid.NewGuid().ToString(),CreateDate= DateTime.Now, City ="mumbai", Country = "india", Phone = "010-85462",PostalCode = "58468", Region = "Kanada", Population = 88822 },
                new Cities {Id = Guid.NewGuid().ToString(),CreateDate= DateTime.Now, City ="nagpur", Country = "Thiland", Phone = "024-586462",PostalCode = "88468", Region = "Asia", Population = 965722 },
                new Cities {Id = Guid.NewGuid().ToString(),CreateDate= DateTime.Now, City ="channai", Country = "india", Phone = "77-222462",PostalCode = "02568", Region = "UK", Population = 485700 },
                new Cities {Id = Guid.NewGuid().ToString(),CreateDate= DateTime.Now, City ="Nashik", Country = "chaina", Phone = "020-5866462",PostalCode = "98438", Region = "Asia", Population = 485445 },
                new Cities {Id = Guid.NewGuid().ToString(),CreateDate= DateTime.Now, City ="Beed", Country = "india", Phone = "440-58600",PostalCode = "85468", Region = "Asia", Population = 968200 },
                new Cities {Id = Guid.NewGuid().ToString(),CreateDate= DateTime.Now, City ="Pune", Country = "Japan", Phone = "320-876462",PostalCode = "58008", Region = "USA", Population = 784242 },
                new Cities {Id = Guid.NewGuid().ToString(),CreateDate= DateTime.Now, City ="mumbai", Country = "india", Phone = "320-02582",PostalCode = "58008", Region = "Poland", Population = 784243 },
                new Cities {Id = Guid.NewGuid().ToString(),CreateDate= DateTime.Now, City ="poland", Country = "Kanda", Phone = "320-74125462",PostalCode = "58008", Region = "USA", Population = 784042 },
                new Cities {Id = Guid.NewGuid().ToString(),CreateDate= DateTime.Now, City ="mosko", Country = "USA", Phone = "320-55462",PostalCode = "88008", Region = "USA", Population = 684242 }
            };

            await _elasticSearchService.InsertBulkDocument("cities", cities);
        }
    }
    
}

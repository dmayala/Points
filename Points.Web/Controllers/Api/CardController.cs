using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Points.Web.Database;

namespace Points.Web.Controllers.Api
{
    [Route("/api/cards")]
    public class CardController : Controller
    {
        private IPointsRepository _repository;
        private ILogger<CardController> _logger;

        public CardController(IPointsRepository repository, ILogger<CardController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var results = _repository.GetAllCards();
            return Json(results);
        }
    }
}

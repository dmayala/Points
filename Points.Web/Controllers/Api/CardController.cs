using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Points.Web.Database;
using System.Net;

namespace Points.Web.Controllers.Api
{
    [Route("/api/cards")]
    public class CardController : Controller
    {
        private readonly IPointsRepository _repository;
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


        [HttpGet]
        [Route("category/{categoryName}")]
        public JsonResult GetBestCardForCategory(string categoryName)
        {
            var categoryValue = _repository.GetBestCardForCategory(categoryName);

            if (categoryValue != null)
            {
                return Json(categoryValue);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed" });
        }

    }
}

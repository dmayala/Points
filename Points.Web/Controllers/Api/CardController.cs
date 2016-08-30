using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Points.Web.Core;

namespace Points.Web.Controllers.Api
{
    [Route("/api/cards")]
    public class CardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CardController> _logger;

        public CardController(IUnitOfWork unitOfWork, ILogger<CardController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var results = _unitOfWork.Cards.GetAllCards();
            return Json(results);
        }
    }
}
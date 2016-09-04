﻿using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Points.Shared.Models;
using Points.Web.Core;

namespace Points.Web.Controllers.Api
{
    [Route("/api/valuations")]
    public class ValuationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ValuationController> _logger;

        public ValuationController(IUnitOfWork unitOfWork, ILogger<ValuationController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [Route("category/{categoryId}")]
        public JsonResult GetBestValuationForCategory(long categoryId)
        {
            var valuation = _unitOfWork.Valuations.GetBestValuationForCategory((Category) categoryId);

            if (valuation != null)
            {
                return Json(valuation);
            }

            Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed" });
        }

        [Route("categories")]
        public JsonResult GetBestValuationsForCategories([FromQuery] string[] category)
        {
            var valuations = _unitOfWork.Valuations.GetBestValuationsForCategories(category);

            if (valuations != null)
            {
                return Json(valuations);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed" });
        }
    }
}
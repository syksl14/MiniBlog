﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniBlog.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View("Error");
        }
        [Route("Error")]
        public ViewResult NotFound()
        {
            Response.StatusCode = 404; 
            return View("NotFound");
        }
    }
}
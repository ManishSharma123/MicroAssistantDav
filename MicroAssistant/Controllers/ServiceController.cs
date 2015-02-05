using MicroAssistant.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MicroAssistant.Lib;
using MicroAssistant.Data;

namespace MicroAssistant.Web.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Service/
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ViewServices()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ServiceDescription()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult AdService()
        {
            AdServiceModel model = new AdServiceModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AdService(AdServiceModel jobmodel)
        {
            if (ModelState.IsValid)
            {
                byte[] photo_aray = null;
                if (jobmodel.Photo != null)
                {
                    Stream file = jobmodel.Photo.InputStream;
                    photo_aray = new byte[file.Length];
                    file.Read(photo_aray, 0, (int)file.Length);
                    file.Close();
                    file.Dispose();
                }
                AdServices.Instance.adService(jobmodel.Id, jobmodel.Title, jobmodel.description, jobmodel.description, photo_aray, jobmodel.StateId, jobmodel.CategoryId, jobmodel.City, "", "");

            }
            return View(jobmodel);
        }
    }
}
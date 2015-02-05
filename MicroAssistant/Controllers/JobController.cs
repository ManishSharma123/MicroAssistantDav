using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MicroAssistant.Web.Models;
using MicroAssistant.Lib;
using MicroAssistant.Data;
using System.IO;
namespace MicroAssistant.Web.Controllers
{
    public class JobController : Controller
    {
        MicroAssistantEntities objcontext = new MicroAssistantEntities();
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult AdPost()
        {
            AdPostModel model = new AdPostModel();
            return View(model);

        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult AdPost(AdPostModel jobmodel)
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

                int IsBestOffer = 0, IsFree = 0;
                if (jobmodel.IsBestOffer)
                {
                    IsBestOffer = 1;
                }
                if (jobmodel.IsFree)
                {
                    IsFree = 1;
                }
                JobService.Instance.adJob(jobmodel.Id, jobmodel.Title, jobmodel.description, jobmodel.description, photo_aray, jobmodel.Video, jobmodel.Price, jobmodel.Phone, jobmodel.City, jobmodel.StateId, jobmodel.CategoryId, IsBestOffer, IsFree, DateTime.UtcNow, DateTime.UtcNow, "", "");
            }
            return View(jobmodel);

        }
        [AllowAnonymous]
        public ActionResult JobDescription()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ViewJobs()
        {
            return View();
        }
    }
}
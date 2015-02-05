using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroAssistant.Web.Controllers
{
    public class MyAccountController : Controller
    {
        //
        // GET: /MyAccount/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /MyAccount/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /MyAccount/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MyAccount/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MyAccount/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MyAccount/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MyAccount/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MyAccount/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

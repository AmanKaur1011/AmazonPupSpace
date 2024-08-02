using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AmazonPupSpace.Models;

namespace AmazonPupSpace.Controllers
{
    public class TrickController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TrickController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44351/api/");
        }
        // GET: Trick
        public ActionResult List()
        {
            //communicate with our Trickdata api to retrieve list of tricks
            //curl https://localhost:44351/api/trickdata/listtricks

            string url = "trickdata/listtricks";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TrickDto> tricks = response.Content.ReadAsAsync<IEnumerable<TrickDto>>().Result;

            return View(tricks);
        }

        // GET: Trick/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Trick/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trick/Create
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

        // GET: Trick/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Trick/Edit/5
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

        // GET: Trick/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Trick/Delete/5
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

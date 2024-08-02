using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using AmazonPupSpace.Models;
using System.Diagnostics;
using System.Web.Script.Serialization;
using AmazonPupSpace.Models;

namespace DogTricksApplication.Controllers
{

    public class TrickController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TrickController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44351/api/trickdata/");
        }
        // GET: Trick
        public ActionResult List()
        {
            //communicate with our Trickdata api to retrieve list of tricks
            //curl https://localhost:44351/api/trickdata/listtricks

            string url = "listtricks";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TrickDto> tricks = response.Content.ReadAsAsync<IEnumerable<TrickDto>>().Result;

            return View(tricks);
        }

        public ActionResult Details(int id)
        {
            //communicate with our Trickdata api to retrieve a trick by id
            //curl https://localhost:44351/api/trickdata/findtrick/9

            string url = "findtrick/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TrickDto selectedTrick = response.Content.ReadAsAsync<TrickDto>().Result;

            return View(selectedTrick);
        }

        [HttpGet]
        // GEt: Trick/New
        public ActionResult New()
        {
            return View();
        }

        // Post: Trick/Create
        [HttpPost]
        public ActionResult Create(Trick trick)
        {
            //For the mvp, the owner is hardcoded, I will add the trick image and trick owner in later stages after the lessons.
            //communicate with our Trickdata api to add a trick
            //curl  -d ""  - "Content-type:application" https://localhost:44351/api/trickdata/AddTrick

            string url = "addtrick";

            string jsonpayload = jss.Serialize(trick);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        //Get:Trick/Update/5
        public ActionResult Update(int id)
        {
            string url = "findtrick/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TrickDto selectedTrick = response.Content.ReadAsAsync<TrickDto>().Result;

            return View(selectedTrick);
        }

        // GET: Trick/Edit/5
        [HttpPost]
        public ActionResult Edit(Trick trick)
        {

            string url = "updatetrick/" + trick.TrickId;

            string jsonpayload = jss.Serialize(trick);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + trick.TrickId);
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Trick/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findtrick/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TrickDto selectedTrick = response.Content.ReadAsAsync<TrickDto>().Result;

            return View(selectedTrick);

        }

        // POST: Trick/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Trick trick)
        {
            try
            {
                // TODO: Add delete logic here
                string url = "deletetrick/" + id;
                HttpContent content = new StringContent("");
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("List");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }


        public ActionResult Error()
        {
            return View();
        }
    }
}

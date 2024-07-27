using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AmazonPupSpace.Models;

namespace AmazonPupSpace.Controllers
{
    public class EmployeeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static EmployeeController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44351/api/");
        }
        // GET: Employee
        [HttpGet]
        public ActionResult List()
        {
            string url = "EmployeeData/ListEmployees";
            HttpResponseMessage response = client.GetAsync(url).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            Debug.WriteLine("Response Body: " + responseBody);
            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<EmployeeDto> employees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            //Debug.WriteLine("Number of employees received : ");
            //Debug.WriteLine(employees.Count());



            return View(employees);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
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

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Employee/Edit/5
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

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
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

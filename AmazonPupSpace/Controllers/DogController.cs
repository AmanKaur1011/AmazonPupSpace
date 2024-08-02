using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AmazonPupSpace.Models;
using AmazonPupSpace.Models.ViewModels;
using System.Diagnostics;

namespace AmazonPupSpace.Controllers
{
    public class DogController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DogController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44351/api/");
        }

        // GET: Dog/List
        public ActionResult List()
        {
            //communicate with our Dogdata api to retrieve list of dogs
            //curl https://localhost:44351/api/dogdata/listdogs

            string url = "dogdata/listdogs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DogDto> dogs = response.Content.ReadAsAsync<IEnumerable<DogDto>>().Result;

            return View(dogs);
        }

        // GET: Dog/Details/5
        public ActionResult Details(int id)
        {
            //communicate with our Dogdata api to retrieve a dog by id
            //curl https://localhost:44366/api/dogdata/finddog/9

            TricksForDog ViewModel = new TricksForDog();
            
            // api request to find a particular dog
            string url = "dogdata/finddog/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DogDto selectedDog = response.Content.ReadAsAsync<DogDto>().Result;
            ViewModel.SelectedDog = selectedDog;

            // api request to   DogxTrick controller to fetch the tricks learnt by that dog so far
            url = "DogxTrickData/ListDogxTricksforDog/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DogxTrickDto> tricksfordog = response.Content.ReadAsAsync<IEnumerable<DogxTrickDto>>().Result;
            ViewModel.TricksLearnt = tricksfordog;

            // pass the data to the view 
            return View(ViewModel);
        }

        //POST: Dog/Associate/{DogId}/{TrickId}
         // This method will associate a trick  to dog
        [HttpPost]
        public ActionResult Associate(int DogId, int  TrickId)
        {
            Debug.WriteLine("Attempting to associate trick  :" + TrickId + " with  Dog " + DogId);

            //call our api to associate trick with Dog
            string url = "DogxTrickdata/AssociateTrickWithDog/" + DogId+ "/" + TrickId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Edit/" + DogId);
        }


        //POST: Dog/UnAssociate/{DogId}/{TrickId}
        // This method will unassociate a trick  to dog
        [HttpPost]
        public ActionResult UnAssociate(int DogId, int TrickId)
        {
            

            //call our api to unassociate trick with dog
            string url = "DogxTrickdata/UnAssociateTrickWithDog/" + DogId + "/" + TrickId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Edit/" + DogId);
        }

        // GET: Dog/New
        [HttpGet]
        
        public ActionResult New()
        {
            AddDog ViewModel = new AddDog();

//---------------- IMPORTANT NOTE----------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------------------------------------
            //I am fetching the list of tricks but i feel we don't need this at the  Add new Pup  page because if the employer wants to add tricks for their  dog they can do that by editing the dog 
            // on the Edit Page so may be we can delete this thing from here
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------
           
            //GET api/TrickData/ListTricks
            //information about all Tricks in the system.
            string url = "TrickData/ListTricks";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<TrickDto> trickOptions = response.Content.ReadAsAsync<IEnumerable<TrickDto>>().Result;
            ViewModel.TrickOptions = trickOptions;

            // api request to fetch  the employees list from the database to add the owner for the dog
            string url2 = "EmployeeData/ListEmployees";
            HttpResponseMessage response2 = client.GetAsync(url2).Result;
            IEnumerable<EmployeeDto> employeeOptions = response2.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            ViewModel.EmployeeOptions = employeeOptions;

            return View(ViewModel);
        }

        // Post: Dog/Create
        [HttpPost]
        public ActionResult Create(Dog dog)
        {
            
            //communicate with our Dogdata api to add a dog
            //curl  -d ""  - "Content-type:application" https://localhost:44351/api/dogdata/AddDog

            string url = "dogdata/adddog";

            string jsonpayload = jss.Serialize(dog);

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

        /// <summary>
        /// Retrieves the details of a specific dog for editing, including the dog's information,
        /// tricks learned by the dog, tricks not yet learned by the dog, and available employees.
        /// </summary>
        /// <param name="id">The ID of the dog to edit.</param>
        /// <returns>An ActionResult that renders the Edit view with the populated UpdateMyDog ViewModel.</returns>   
        // GET: Dog/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Edit page requires
            UpdateMyDog ViewModel = new UpdateMyDog();

            // Retrieve the existing Dog information
            string url = "DogData/FindDog/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DogDto SelectedDog = response.Content.ReadAsAsync<DogDto>().Result;
            ViewModel.SelectedDog = SelectedDog;

            // Retrieve the tricks learned by the dog
            url = "DogxTrickData/ListDogxTricksforDog/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DogxTrickDto> tricksfordog = response.Content.ReadAsAsync<IEnumerable<DogxTrickDto>>().Result;
            ViewModel.TricksLearnt = tricksfordog;

            // Retrieve the tricks not yet learned by the dog
            url = "TrickData/ListTricks";
             response = client.GetAsync(url).Result;
            IEnumerable<TrickDto> trickOptions = response.Content.ReadAsAsync<IEnumerable<TrickDto>>().Result;
            Debug.WriteLine(trickOptions);
            ViewModel.AvailableTrickOptions = trickOptions;

             // Retrieve the list of available employees
             url = "EmployeeData/ListEmployees";
             response = client.GetAsync(url).Result;
            IEnumerable<EmployeeDto> employeeOptions = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            ViewModel.EmployeeOptions = employeeOptions;

            // Return the populated ViewModel to the Edit view
            return View(ViewModel);
        }

        // POST: Dog/Edit/5
        [HttpPost]
        public ActionResult Update( Dog dog)
        {
            string url = "DogData/UpdateDog/" + dog.DogId;

            string jsonpayload = jss.Serialize(dog);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + dog.DogId);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "dogdata/finddog/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DogDto selectedDog = response.Content.ReadAsAsync<DogDto>().Result;

            return View(selectedDog);

        }

        // POST: Dog/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                
                string url = "dogdata/deletedog/" + id;
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

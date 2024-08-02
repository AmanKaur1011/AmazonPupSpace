using AmazonPupSpace.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace AmazonPupSpace.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CommentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44351/api/");
        }

        // GET: Comment/List
        public ActionResult List()
        {
            // OBJECTIVE: Communication with the comments data API to retrieve a list of comments.
            // curl https://localhost:44351/api/commentsdata/listcomments
            string url = "commentdata/listcomments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Deserialize the JSON response into an IEnumerable of Comments objects.
            IEnumerable<CommentDto> comments = response.Content.ReadAsAsync<IEnumerable<CommentDto>>().Result;

            // Pass the list of comments to the view for rendering.
            return View(comments);
        }

        // GET: Comment/Details/5
        public ActionResult Details(int id)
        {
            // OBJECTIVE: Communication with the comments data API to retrieve a specific comment.
            // curl https://localhost:44351/api/commentsdata/findcomment/{id}
            string url = "commentdata/findcomment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Deserialize the JSON response into a Comments object.
            CommentDto SelectedComment = response.Content.ReadAsAsync<CommentDto>().Result;

            // Pass the comment object to the view for rendering.
            return View(SelectedComment);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Comment/Create
        public ActionResult New()
        {
            return View();
        }

        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            // Define the API endpoint for adding a new comment.
            string url = "commentdata/addcomment";

            // Serialize the comments object to JSON format.
            string jsonpayload = jss.Serialize(comment);
            Debug.WriteLine(jsonpayload);

            // Create a new HttpContent object for the JSON payload.
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            // Send the POST request to the API endpoint.
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                // If the request is successful, redirect to the commented art.
                return RedirectToAction("Details", "Post", new { id = comment.PostId }); // Redirect to Art Details view
            }
            else
            {
                // If the request fails, redirect to the error view.
                return RedirectToAction("Error");
            }
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int id)
        {
            // Define the API endpoint for finding a comment by ID.
            string url = "commentdata/findcomment/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;

            // Deserialize the JSON response into a Comments object.
            CommentDto SelectedComment = responseMessage.Content.ReadAsAsync<CommentDto>().Result;

            // Pass the comment object to the view for editing.
            return View(SelectedComment);
        }

        // POST: Comment/Update/5
        [HttpPost]
        public ActionResult Update(int id, Comment comment)
        {
            // Define the API endpoint for updating a comment.
            string url = "commentdata/updatecomment/" + id;

            // Serialize the comments object to JSON format.
            string jsonpayload = jss.Serialize(comment);

            // Create a new HttpContent object for the JSON payload.
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            // Send the POST request to the API endpoint.
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                // If the request is successful, redirect to the commented art.
                return RedirectToAction("Details", "Post", new { id = comment.PostId }); // Redirect to Art Details view
            }
            else
            {
                // If the request fails, redirect to the error view.
                return RedirectToAction("Error");
            }
        }

        // GET: Comment/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            // Define the API endpoint for finding a comment by ID.
            string url = "commentdata/findcomment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Deserialize the JSON response into a Comments object.
            CommentDto SelectedComment = response.Content.ReadAsAsync<CommentDto>().Result;

            if (SelectedComment == null)
            {
                // If the comment is not found, redirect to the error view.
                return RedirectToAction("Error");
            }

            // Pass the comment object to the view for deletion confirmation.
            return View(SelectedComment);
        }

        // POST: Commentt/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // Define the API endpoint for deleting a comment by ID.
            string url = "commentdata/deletecomment/" + id;

            // Create an empty HttpContent object since the delete request doesn't require a body.
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            // Send the POST request to the API endpoint.
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                // If the request is successful, redirect to the comments list.
                return RedirectToAction("List");
            }
            else
            {
                // If the request fails, redirect to the error view.
                return RedirectToAction("Error");
            }
        }
    }
}

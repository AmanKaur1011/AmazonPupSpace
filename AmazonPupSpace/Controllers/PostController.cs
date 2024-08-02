using AmazonPupSpace.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls.WebParts;
using AmazonPupSpace.Models.ViewModels;

namespace AmazonPupSpace.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PostController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44351/api/");
        }
        // GET: Post/List
        public ActionResult List(string searchQuery)
        {
            var posts = db.Posts.AsQueryable();

            // Filter the results based on the search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                posts = posts.Where(a => a.Title.Contains(searchQuery));
            }

            return View(posts.ToList());
        }

        // GET: Post/Details/5
        public ActionResult Details(int id)
        {
            PostDetailsViewModel ViewModel = new PostDetailsViewModel();

            // Fetch the art piece
            string url = "postdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                Post SelectedPost = response.Content.ReadAsAsync<Post>().Result;
                ViewModel.Post = SelectedPost;
            }
            else
            {
                return HttpNotFound("Art piece not found.");
            }

            // Fetch the comments
            url = "commentdata/ListCommentsForPost/" + id;
            response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<CommentDto> RelatedComments = response.Content.ReadAsAsync<IEnumerable<CommentDto>>().Result;
                ViewModel.Comments = RelatedComments;
            }
            else
            {
                ViewModel.Comments = new List<CommentDto>();
            }

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Art/New
        public ActionResult New()
        {
            return View();
        }

        // GET: Post/Create
        public ActionResult Create(Post post)
        {
            string url = "postdata/addpost";

            // Serialize the post object to JSON format.
            string jsonpayload = jss.Serialize(post);
            Debug.WriteLine(jsonpayload);

            // Create a new HttpContent object for the JSON payload.
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            // Send the POST request to the API endpoint.
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                // If the request is successful, redirect to the post list.
                return RedirectToAction("List");
            }
            else
            {
                // If the request fails, redirect to the error view.
                return RedirectToAction("Error");
            }
        }

        // GET: Post/Edit/5
        public ActionResult Edit(int id)
        {
            // Define the API endpoint for finding an post piece by ID.
            string url = "postdata/findpost/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;

            // Deserialize the JSON response into an Art object.
            Post SelectedPost = responseMessage.Content.ReadAsAsync<Post>().Result;

            // Pass the post piece object to the view for editing.
            return View(SelectedPost);
        }

        // POST: Post/Update/5
        [HttpPost]
        public ActionResult Update(int id, Post post, HttpPostedFileBase ImageURL)
        {
            // Define the API endpoint for updating an post piece.
            string url = "postdata/updatepost/" + id;

            // Serialize the post object to JSON format.
            string jsonpayload = jss.Serialize(post);

            // Create a new HttpContent object for the JSON payload.
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            // Send the POST request to the API endpoint.
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode && ImageURL != null)
            {//Updating the animal picture as a separate request
                Debug.WriteLine("Calling Update Image method.");
                //Send over image data for player
                url = "PostData/UploadPostPic/" + id;

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(ImageURL.InputStream);
                requestcontent.Add(imagecontent, "ImageURL", ImageURL.FileName);
                response = client.PostAsync(url, requestcontent).Result;
                // If the request is successful, redirect to the post edited.
                return RedirectToAction("Details", "Post", new { id = post.PostId });
            }
            else if (response.IsSuccessStatusCode)
            {
                //No image upload, but update still successful
                return RedirectToAction("Details", "Post", new { id = post.PostId });
            }
            else
            {
                // If the request fails, redirect to the error view.
                return RedirectToAction("Error");
            }
        }

        // GET: Post/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "postdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Deserialize the JSON response into an Art object.
            Post SelectedPost = response.Content.ReadAsAsync<Post>().Result;

            // Pass the post piece object to the view for deletion confirmation.
            return View(SelectedPost);
        }

        // POST: Post/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "postdata/deletepost/" + id;

            // Create an empty HttpContent object since the delete request doesn't require a body.
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            // Send the POST request to the API endpoint.
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                // If the request is successful, redirect to the art list.
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

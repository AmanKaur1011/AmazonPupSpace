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
using Microsoft.AspNet.Identity;

namespace AmazonPupSpace.Controllers
{
    /// <summary>
    /// Manages CRUD operations for posts.
    /// </summary>
    public class PostController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PostController()
        {
            // Initialize the HttpClient with the base address for API calls.
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44351/api/");
        }

        /// <summary>
        /// Displays a list of posts, optionally filtered by a search query.
        /// </summary>
        /// <param name="searchQuery">The query to filter posts by title.</param>
        /// <returns>A view with a list of posts.</returns>
        public ActionResult List(string searchQuery)
        {
            var posts = db.Posts.AsQueryable();

            // Filter the posts based on the search query if provided.
            if (!string.IsNullOrEmpty(searchQuery))
            {
                posts = posts.Where(a => a.Title.Contains(searchQuery));
            }

            return View(posts.ToList());
        }

        /// <summary>
        /// Displays the details of a specific post, including its comments.
        /// </summary>
        /// <param name="id">The ID of the post to display.</param>
        /// <returns>A view with details of the post.</returns>
        public ActionResult Details(int id)
        {
            PostDetailsViewModel ViewModel = new PostDetailsViewModel();

            // Fetch the post details from the API.
            string url = "postdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var postDto = response.Content.ReadAsAsync<PostDto>().Result;
                ViewModel.Post = new Post
                {
                    PostId = postDto.PostId,
                    Title = postDto.Title,
                    Caption = postDto.Caption,
                    ImageURL = postDto.ImageURL,
                    PicExtension = postDto.PicExtension,
                    PostDate = postDto.PostDate,
                    EmployeeId = postDto.EmployeeId
                };
                ViewModel.Employee = new Employee
                {
                    FirstName = postDto.FirstName,
                    LastName = postDto.LastName,
                    Email = postDto.Email
                };
            }
            else
            {
                return HttpNotFound("Art piece not found.");
            }

            // Fetch comments related to the post.
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

        /// <summary>
        /// Displays the error view.
        /// </summary>
        /// <returns>An error view.</returns>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Displays a form to create a new post.
        /// </summary>
        /// <returns>A view with a form to create a new post.</returns>
        public ActionResult New()
        {
            // Get the current user's ID and fetch employee details.
            string userId = User.Identity.GetUserId();
            var employee = db.Employees.FirstOrDefault(e => e.UserId == userId);
            ViewBag.EmployeeId = employee?.EmployeeId;

            return View();
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="post">The post to create.</param>
        /// <returns>A redirection to the post list or an error view.</returns>
        [HttpPost]
        public ActionResult Create(Post post)
        {
            // Ensure the EmployeeId is provided and valid.
            if (string.IsNullOrEmpty(Request.Form["EmployeeId"]) || !int.TryParse(Request.Form["EmployeeId"], out int employeeId))
            {
                return RedirectToAction("Error");
            }

            post.EmployeeId = employeeId;
            string url = "postdata/addpost";

            // Serialize and send the new post to the API.
            string jsonpayload = jss.Serialize(post);
            Debug.WriteLine(jsonpayload);
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
        /// Displays a form to edit an existing post.
        /// </summary>
        /// <param name="id">The ID of the post to edit.</param>
        /// <returns>A view with the post details for editing.</returns>
        public ActionResult Edit(int id)
        {
            // Fetch the post data for editing.
            string url = "postdata/findpost/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;
            Post SelectedPost = responseMessage.Content.ReadAsAsync<Post>().Result;

            return View(SelectedPost);
        }

        /// <summary>
        /// Updates an existing post.
        /// </summary>
        /// <param name="id">The ID of the post to update.</param>
        /// <param name="post">The updated post details.</param>
        /// <param name="ImageURL">The new image for the post (if any).</param>
        /// <returns>A redirection to the post details or an error view.</returns>
        [HttpPost]
        public ActionResult Update(int id, Post post, HttpPostedFileBase ImageURL)
        {
            // Update the post details.
            string url = "postdata/updatepost/" + id;
            string jsonpayload = jss.Serialize(post);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                // If an image is uploaded, handle the image update separately.
                if (ImageURL != null)
                {
                    Debug.WriteLine("Calling Update Image method.");
                    url = "PostData/UploadPostPic/" + id;
                    MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                    HttpContent imagecontent = new StreamContent(ImageURL.InputStream);
                    requestcontent.Add(imagecontent, "ImageURL", ImageURL.FileName);
                    response = client.PostAsync(url, requestcontent).Result;
                }

                return RedirectToAction("Details", new { id = post.PostId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Displays a confirmation view before deleting a post.
        /// </summary>
        /// <param name="id">The ID of the post to delete.</param>
        /// <returns>A view with the post details for deletion confirmation.</returns>
        public ActionResult DeleteConfirm(int id)
        {
            // Fetch the post data to confirm deletion.
            string url = "postdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Post SelectedPost = response.Content.ReadAsAsync<Post>().Result;

            return View(SelectedPost);
        }

        /// <summary>
        /// Deletes a post.
        /// </summary>
        /// <param name="id">The ID of the post to delete.</param>
        /// <param name="collection">Form collection (not used).</param>
        /// <returns>A redirection to the post list or an error view.</returns>
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // Delete the post.
            string url = "postdata/deletepost/" + id;
            HttpContent content = new StringContent("");
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
    }
}

using AmazonPupSpace.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AmazonPupSpace.Controllers
{
    /// <summary>
    /// The CommentController is responsible for managing comments within the AmazonPupSpace application.
    /// </summary>
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

        /// <summary>
        /// Retrieves a list of all comments.
        /// </summary>
        /// <returns>
        /// A view displaying all comments.
        /// </returns>
        /// <example>
        /// GET: Comment/List
        /// </example>
        public ActionResult List()
        {
            string url = "commentdata/listcomments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CommentDto> comments = response.Content.ReadAsAsync<IEnumerable<CommentDto>>().Result;

            return View(comments);
        }

        /// <summary>
        /// Retrieves the details of a specific comment.
        /// </summary>
        /// <param name="id">The ID of the comment.</param>
        /// <returns>
        /// A view displaying the details of the specified comment.
        /// </returns>
        /// <example>
        /// GET: Comment/Details/5
        /// </example>
        public ActionResult Details(int id)
        {
            string url = "commentdata/findcomment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CommentDto SelectedComment = response.Content.ReadAsAsync<CommentDto>().Result;

            return View(SelectedComment);
        }

        /// <summary>
        /// Displays an error view.
        /// </summary>
        /// <returns>
        /// A view displaying an error message.
        /// </returns>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Displays the form for creating a new comment.
        /// </summary>
        /// <returns>
        /// A view displaying the form for creating a new comment.
        /// </returns>
        /// <example>
        /// GET: Comment/Create
        /// </example>
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="comment">The comment object to create.</param>
        /// <returns>
        /// A redirection to the details view of the commented post if successful, or an error view if not.
        /// </returns>
        /// <example>
        /// POST: Comment/Create
        /// FORM DATA: New Comment JSON Object
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var employee = db.Employees.SingleOrDefault(e => e.UserId == userId);

                if (employee != null)
                {
                    comment.EmployeeId = employee.EmployeeId;
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Employee not found.");
                }

                string url = "commentdata/addcomment";
                string jsonpayload = jss.Serialize(comment);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", "Post", new { id = comment.PostId });
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
        }

        /// <summary>
        /// Displays the form for editing an existing comment.
        /// </summary>
        /// <param name="id">The ID of the comment to edit.</param>
        /// <returns>
        /// A view displaying the form for editing the specified comment.
        /// </returns>
        /// <example>
        /// GET: Comment/Edit/5
        /// </example>
        public ActionResult Edit(int id)
        {
            string url = "commentdata/findcomment/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;

            CommentDto SelectedComment = responseMessage.Content.ReadAsAsync<CommentDto>().Result;

            return View(SelectedComment);
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="id">The ID of the comment to update.</param>
        /// <param name="comment">The updated comment object.</param>
        /// <returns>
        /// A redirection to the details view of the commented post if successful, or an error view if not.
        /// </returns>
        /// <example>
        /// POST: Comment/Update/5
        /// FORM DATA: Updated Comment JSON Object
        /// </example>
        [HttpPost]
        public ActionResult Update(int id, Comment comment)
        {
            string url = "commentdata/updatecomment/" + id;
            string jsonpayload = jss.Serialize(comment);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", "Post", new { id = comment.PostId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Displays the form for confirming the deletion of a comment.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>
        /// A view displaying the deletion confirmation for the specified comment.
        /// </returns>
        /// <example>
        /// GET: Comment/DeleteConfirm/5
        /// </example>
        public ActionResult DeleteConfirm(int id)
        {
            string url = "commentdata/findcomment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CommentDto SelectedComment = response.Content.ReadAsAsync<CommentDto>().Result;

            if (SelectedComment == null)
            {
                return RedirectToAction("Error");
            }

            return View(SelectedComment);
        }

        /// <summary>
        /// Deletes a specific comment.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <param name="collection">The form collection containing additional data (unused).</param>
        /// <returns>
        /// A redirection to the comments list if successful, or an error view if not.
        /// </returns>
        /// <example>
        /// POST: Comment/Delete/5
        /// </example>
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "commentdata/deletecomment/" + id;

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

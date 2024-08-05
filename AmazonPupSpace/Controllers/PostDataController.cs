using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using AmazonPupSpace.Models;

namespace AmazonPupSpace.Controllers
{
    /// <summary>
    /// The PostDataController is responsible for handling CRUD operations for posts in the AmazonPupSpace application.
    /// </summary>
    public class PostDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all posts.
        /// </summary>
        /// <returns>An IQueryable list of Post objects.</returns>
        /// <example>
        /// GET: api/PostData/ListPosts
        /// </example>
        [HttpGet]
        public IQueryable<Post> ListPosts()
        {
            return db.Posts;
        }

        /// <summary>
        /// Retrieves a list of posts by a specific employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>A list of PostDto objects created by the specified employee.</returns>
        /// <example>
        /// GET: api/postdata/listpostsbyemployee/{employeeId}
        /// </example>
        [Route("api/postdata/listpostsbyemployee/{employeeId}")]
        [HttpGet]
        public IHttpActionResult GetPostsByEmployee(int employeeId)
        {
            var posts = db.Posts.Where(p => p.EmployeeId == employeeId).ToList();

            if (posts == null || !posts.Any())
            {
                return NotFound();
            }

            var postDtos = posts.Select(p => new PostDto
            {
                PostId = p.PostId,
                Title = p.Title,
                Caption = p.Caption,
                ImageURL = p.ImageURL,
                PicExtension = p.PicExtension,
                PostDate = p.PostDate
            });

            return Ok(postDtos);
        }

        /// <summary>
        /// Retrieves the details of a specific post.
        /// </summary>
        /// <param name="id">The ID of the post.</param>
        /// <returns>A PostDto object containing the details of the specified post.</returns>
        /// <example>
        /// GET: api/PostData/FindPost/5
        /// </example>
        [ResponseType(typeof(Post))]
        [HttpGet]
        public IHttpActionResult FindPost(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            PostDto postDto = new PostDto()
            {
                PostId = post.PostId,
                Title = post.Title,
                Caption = post.Caption,
                ImageURL = post.ImageURL,
                PicExtension = post.PicExtension,
                PostDate = post.PostDate,
                EmployeeId = post.Employee.EmployeeId,
                FirstName = post.Employee.FirstName,
                LastName = post.Employee.LastName,
            };

            return Ok(postDto);
        }

        /// <summary>
        /// Updates an existing post.
        /// </summary>
        /// <param name="id">The ID of the post to update.</param>
        /// <param name="post">The updated post object.</param>
        /// <returns>Status code 204 (No Content) if the update is successful, or an error response if not.</returns>
        /// <example>
        /// PUT: api/PostData/5
        /// FORM DATA: Updated Post JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePost(int id, Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != post.PostId)
            {
                return BadRequest();
            }

            db.Entry(post).State = EntityState.Modified;
            db.Entry(post).Property(p => p.ImageURL).IsModified = false;
            db.Entry(post).Property(p => p.PicExtension).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Receives post picture data, uploads it to the webserver, and updates the post's picture information.
        /// </summary>
        /// <param name="id">The post ID.</param>
        /// <returns>Status code 200 (OK) if the upload is successful, or a bad request response if not.</returns>
        /// <example>
        /// curl -F artpic=@file.jpg "https://localhost:xx/api/artdata/uploadpostpic/2"
        /// POST: api/artData/UpdatepostPic/3
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        [HttpPost]
        public IHttpActionResult UploadPostPic(int id)
        {
            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var artPic = HttpContext.Current.Request.Files[0];
                    if (artPic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(artPic.FileName).Substring(1);
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                string fn = id + "." + extension;
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Posts/"), fn);
                                artPic.SaveAs(path);

                                haspic = true;
                                picextension = extension;

                                Post Selectedpost = db.Posts.Find(id);
                                Selectedpost.ImageURL = haspic;
                                Selectedpost.PicExtension = extension;
                                db.Entry(Selectedpost).State = EntityState.Modified;

                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Post Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }
                }

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Adds a new post.
        /// </summary>
        /// <param name="post">The post object to add.</param>
        /// <returns>Status code 201 (Created) if the post is successfully added, or a bad request response if not.</returns>
        /// <example>
        /// POST: api/PostData/AddPost
        /// FORM DATA: New Post JSON Object
        /// </example>
        [ResponseType(typeof(Post))]
        [HttpPost]
        public IHttpActionResult AddPost(Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Posts.Add(post);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = post.PostId }, post);
        }

        /// <summary>
        /// Deletes a specific post.
        /// </summary>
        /// <param name="id">The ID of the post to delete.</param>
        /// <returns>Status code 200 (OK) if the post is successfully deleted, or a not found response if the post does not exist.</returns>
        /// <example>
        /// POST: api/PostData/DeletePost/5
        /// </example>
        [ResponseType(typeof(Post))]
        [HttpPost]
        public IHttpActionResult DeletePost(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            if (post.ImageURL && post.PicExtension != "")
            {
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/Posts/" + id + "." + post.PicExtension);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists... preparing to delete!");
                    System.IO.File.Delete(path);
                }
            }

            db.Posts.Remove(post);
            db.SaveChanges();

            return Ok(post);
        }

        /// <summary>
        /// Disposes of the database context when the controller is disposed.
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether the context should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Checks if a post with the specified ID exists.
        /// </summary>
        /// <param name="id">The ID of the post.</param>
        /// <returns>True if the post exists, false otherwise.</returns>
        private bool PostExists(int id)
        {
            return db.Posts.Count(e => e.PostId == id) > 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AmazonPupSpace.Models;

namespace AmazonPupSpace.Controllers
{
    /// <summary>
    /// The CommentDataController is responsible for handling CRUD operations related to comments in the AmazonPupSpace application.
    /// </summary>
    public class CommentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all comments in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All comments in the database.
        /// </returns>
        /// <example>
        /// GET: api/CommentData/ListComments
        /// </example>
        [HttpGet]
        [Route("api/CommentData/ListComments")]
        [ResponseType(typeof(CommentDto))]
        public IHttpActionResult ListComments()
        {
            List<Comment> Comments = db.Comments.ToList();
            List<CommentDto> CommentDtos = new List<CommentDto>();

            Comments.ForEach(p => CommentDtos.Add(new CommentDto()
            {
                CommentId = p.CommentId,
                CommentText = p.CommentText,
                DateCommented = p.DateCommented,
                PostId = p.Post.PostId,
                Title = p.Post.Title,
                ImageURL = p.Post.ImageURL,
                PicExtension = p.Post.PicExtension,
                EmployeeId = p.Employee.EmployeeId,
                FirstName = p.Employee.FirstName,
                LastName = p.Employee.LastName,
            }));

            return Ok(CommentDtos);
        }

        /// <summary>
        /// Returns comments for a specific post.
        /// </summary>
        /// <param name="id">The ID of the post.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Comments related to the specified post.
        /// </returns>
        /// <example>
        /// GET: api/CommentData/ListCommentsForPost/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CommentDto))]
        public IEnumerable<CommentDto> ListCommentsForPost(int id)
        {
            var comments = db.Comments
                             .Where(c => c.PostId == id)
                             .Include(c => c.Employee)
                             .Select(c => new CommentDto
                             {
                                 CommentId = c.CommentId,
                                 CommentText = c.CommentText,
                                 DateCommented = c.DateCommented,
                                 PostId = c.Post.PostId,
                                 Title = c.Post.Title,
                                 EmployeeId = c.Employee.EmployeeId,
                                 FirstName = c.Employee.FirstName,
                                 LastName = c.Employee.LastName
                             });

            return comments.ToList();
        }

        /// <summary>
        /// Finds a specific comment by ID.
        /// </summary>
        /// <param name="id">The ID of the comment.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The comment matching the specified ID.
        /// HEADER: 404 (Not Found)
        /// CONTENT: No comment found with the specified ID.
        /// </returns>
        /// <example>
        /// GET: api/CommentData/FindComment/5
        /// </example>
        [ResponseType(typeof(CommentDto))]
        [HttpGet]
        public IHttpActionResult FindComment(int id)
        {
            Comment Comment = db.Comments.Find(id);
            if (Comment == null)
            {
                return NotFound();
            }

            CommentDto CommentsDto = new CommentDto()
            {
                CommentId = Comment.CommentId,
                CommentText = Comment.CommentText,
                DateCommented = Comment.DateCommented,
                PostId = Comment.Post.PostId,
                Title = Comment.Post.Title,
                Caption = Comment.Post.Caption,
                ImageURL = Comment.Post.ImageURL,
                PicExtension = Comment.Post.PicExtension,
                EmployeeId = Comment.Employee.EmployeeId,
                FirstName = Comment.Employee.FirstName,
                LastName = Comment.Employee.LastName,
            };

            return Ok(CommentsDto);
        }

        /// <summary>
        /// Updates a specific comment.
        /// </summary>
        /// <param name="id">The ID of the comment to be updated.</param>
        /// <param name="comment">The updated comment data.</param>
        /// <returns>
        /// HEADER: 204 (No Content)
        /// CONTENT: No content.
        /// HEADER: 400 (Bad Request)
        /// CONTENT: If the ID does not match or the model state is invalid.
        /// HEADER: 404 (Not Found)
        /// CONTENT: If no comment is found with the specified ID.
        /// </returns>
        /// <example>
        /// POST: api/CommentData/UpdateComment/5
        /// FORM DATA: Updated Comment JSON Object
        /// </example>
        [ResponseType(typeof(CommentDto))]
        [HttpPost]
        public IHttpActionResult UpdateComment(int id, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.CommentId)
            {
                return BadRequest();
            }

            db.Entry(comment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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
        /// Adds a new comment.
        /// </summary>
        /// <param name="comment">The new comment data.</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: The newly created comment.
        /// HEADER: 400 (Bad Request)
        /// CONTENT: If the model state is invalid.
        /// </returns>
        /// <example>
        /// POST: api/CommentData/AddComment
        /// FORM DATA: New Comment JSON Object
        /// </example>
        [ResponseType(typeof(Comment))]
        [HttpPost]
        public IHttpActionResult AddComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Comments.Add(comment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = comment.CommentId }, comment);
        }

        /// <summary>
        /// Deletes a specific comment by ID.
        /// </summary>
        /// <param name="id">The ID of the comment to be deleted.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The deleted comment.
        /// HEADER: 404 (Not Found)
        /// CONTENT: If no comment is found with the specified ID.
        /// </returns>
        /// <example>
        /// POST: api/CommentData/DeleteComment/5
        /// </example>
        [ResponseType(typeof(Comment))]
        [HttpPost]
        public IHttpActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            db.Comments.Remove(comment);
            db.SaveChanges();

            return Ok(comment);
        }

        /// <summary>
        /// Disposes the database context.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Checks if a comment exists by ID.
        /// </summary>
        /// <param name="id">The ID of the comment.</param>
        /// <returns>True if the comment exists, otherwise false.</returns>
        private bool CommentExists(int id)
        {
            return db.Comments.Count(e => e.CommentId == id) > 0;
        }
    }
}

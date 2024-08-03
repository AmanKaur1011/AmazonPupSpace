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
    public class CommentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CommentData/ListComments
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

        // GET: api/CommentData/findComment/5
        [ResponseType(typeof(CommentDto))]
        [HttpGet]
        public IHttpActionResult FindComment(int id)
        {
            Comment Comment = db.Comments.Find(id);
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
            if (Comment == null)
            {
                return NotFound();
            }

            return Ok(CommentsDto);
        }

        // PUT: api/CommentData/UpdateComment/5
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

        // POST: api/CommentData/AddComment
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

        // DELETE: api/CommentData/DeleteComment/5
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(int id)
        {
            return db.Comments.Count(e => e.CommentId == id) > 0;
        }
    }
}
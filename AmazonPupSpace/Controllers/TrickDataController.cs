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
    public class TrickDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        ///grabs all tricks in the database

        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT:all tricks
        /// </returns>
        /// <example>
        /// GET: api/TrickData/ListTricks
        /// </example>

        [HttpGet]
        public IEnumerable<TrickDto> ListTricks()
        {
            List<Trick> Tricks = db.Tricks.ToList();
            List<TrickDto> TrickDtos = new List<TrickDto>();

            Tricks.ForEach( trick => TrickDtos.Add(new TrickDto()
            {
                TrickId = trick.TrickId,
                TrickName = trick.TrickName,
                TrickDescription = trick.TrickDescription,
                TrickDifficulty = trick.TrickDifficulty,
                TrickVideoLink = trick.TrickVideoLink
            }));
            return TrickDtos;
        }

        [HttpGet]
        [Route("api/TrickData/ListTricksNotLearnedByDog/{DogId}")]
        public IEnumerable<TrickDto> ListTricksNotLearnedByDog(int DogId)
        {
            // Retrieve all available tricks
            List<Trick> allTricks = db.Tricks.ToList();

            // Retrieve tricks learned by the specific dog
            List<int> learnedTrickIds = db.DogxTricks
                .Where(dt => dt.DogId == DogId)
                .Select(dt => dt.TrickId)
                .ToList();

            // Find tricks that are not learned by the dog
            List<Trick> tricksNotLearned = allTricks
                .Where(trick => !learnedTrickIds.Contains(trick.TrickId))
                .ToList();

            // Map to DTOs
            List<TrickDto> trickDtos = tricksNotLearned.Select(trick => new TrickDto
            {
                TrickId = trick.TrickId,
                TrickName = trick.TrickName,
                TrickDescription = trick.TrickDescription,
                TrickDifficulty = trick.TrickDifficulty,
                TrickVideoLink = trick.TrickVideoLink
            }).ToList();

            return trickDtos;
        }


        // GET: api/TrickData/5
        [ResponseType(typeof(Trick))]
        public IHttpActionResult GetTrick(int id)
        {
            Trick trick = db.Tricks.Find(id);
            if (trick == null)
            {
                return NotFound();
            }

            return Ok(trick);
        }

        // PUT: api/TrickData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTrick(int id, Trick trick)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trick.TrickId)
            {
                return BadRequest();
            }

            db.Entry(trick).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrickExists(id))
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

        // POST: api/TrickData
        [ResponseType(typeof(Trick))]
        public IHttpActionResult PostTrick(Trick trick)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tricks.Add(trick);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = trick.TrickId }, trick);
        }

        // DELETE: api/TrickData/5
        [ResponseType(typeof(Trick))]
        public IHttpActionResult DeleteTrick(int id)
        {
            Trick trick = db.Tricks.Find(id);
            if (trick == null)
            {
                return NotFound();
            }

            db.Tricks.Remove(trick);
            db.SaveChanges();

            return Ok(trick);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrickExists(int id)
        {
            return db.Tricks.Count(e => e.TrickId == id) > 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
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

            Tricks.ForEach(trick => TrickDtos.Add(new TrickDto()
            {
                TrickId = trick.TrickId,
                TrickName = trick.TrickName,
                TrickDescription = trick.TrickDescription,
                TrickDifficulty = trick.TrickDifficulty,
                TrickVideoLink = trick.TrickVideoLink
            }));
            return TrickDtos;
        }


        /// <summary>
        ///grabs one trick by id  in the database
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT:one trick
        /// </returns>
        /// <example>
        /// GET: api/TrickData/FindTrick/5
        /// </example>

        [ResponseType(typeof(Trick))]
        [HttpGet]
        public IHttpActionResult FindTrick(int id)
        {
            Trick trick = db.Tricks.Find(id);
            if (trick == null)
            {
                return NotFound();
            }
            TrickDto TrickDto = new TrickDto()
            {
                TrickId = trick.TrickId,
                TrickName = trick.TrickName,
                TrickDescription = trick.TrickDescription,
                TrickDifficulty = trick.TrickDifficulty,
                TrickVideoLink = trick.TrickVideoLink
            };

            return Ok(TrickDto);
        }

        /// <summary>
        ///grabs onedogxtrick per trick
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT:one dogxtrick
        /// </returns>
        /// <example>
        /// GET: api/TrickData/ ListTricksforDog/9
        /// </example>

        [HttpGet]
        public TrickDto ListTricksforDog(int id)
        {
            //all dogs that have dogtricks that match with our id
            List<Trick> tricks = db.Tricks.Where(trick => trick.Dogs.Any(dogtrick => dogtrick.DogTrickId == id)).ToList();
            List<TrickDto> TrickDtos = new List<TrickDto>();

            tricks.ForEach(trick => TrickDtos.Add(new TrickDto()
            {
                TrickId = trick.TrickId,
                TrickName = trick.TrickName,
                TrickDescription = trick.TrickDescription,
                TrickDifficulty = trick.TrickDifficulty,
                TrickVideoLink = trick.TrickVideoLink
            }));

            return TrickDtos.First();
        }


        /// <summary>
        ///updates oner trick
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT:one trick
        /// </returns>
        /// <example>
        /// PUT: api/TrickData/UpdateTrick/5
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTrick(int id, Trick trick)
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

        /// <summary>
        ///adds ones trick
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT:one trick
        /// </returns>
        /// <example>
        /// POST: api/TrickData/AddTrick
        /// </example>

        [ResponseType(typeof(Trick))]
        [HttpPost]
        public IHttpActionResult AddTrick(Trick trick)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tricks.Add(trick);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = trick.TrickId }, trick);
        }



        /// <summary>
        ///adds ones trick
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT:one trick
        /// </returns>
        /// <example>
        ///  DELETE: api/TrickData/DeleteTrick
        /// </example>

        [ResponseType(typeof(Trick))]
        [HttpPost]
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
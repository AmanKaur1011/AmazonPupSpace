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
    public class DogxTrickDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        /// <summary>
        /// lists all dog tricks per dog
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all dogtricks per dog
        /// </returns>
        /// <example>
        ///GET: api/DogxTrickData/ListDogxTricksforDog/id
        /// </example>
        [HttpGet]
        public IEnumerable<DogxTrickDto> ListDogxTricksforDog(int id)
        {
            //all dogs that have dogtricks that match with our id
            List<DogxTrick> DogxTricks = db.DogxTricks.Where(d=> d.DogId == id).ToList();

            List<DogxTrickDto> DogTrickDtos = new List<DogxTrickDto>();

             foreach(DogxTrick dogtrick in DogxTricks)
            {
                DogxTrickDto dogtrickdto= new DogxTrickDto();

                dogtrickdto.DogTrickId = dogtrick.DogTrickId;
                dogtrickdto.DogTrickDate = dogtrick.DogTrickDate;
                dogtrickdto.DogId = dogtrick.DogId;
                dogtrickdto.TrickId = dogtrick.TrickId;
                dogtrickdto.TrickName = dogtrick.Trick.TrickName;

                DogTrickDtos.Add(dogtrickdto);
                        
            }
            
            return DogTrickDtos;
        }

        [HttpGet]
        public IEnumerable<DogxTrickDto> ListTricksNotLearnedByDog(int id)
        {
            //all dogs that have dogtricks that match with our id
            List<DogxTrick> DogxTricks = db.DogxTricks.Where(d => d.DogId != id).ToList();

            List<DogxTrickDto> DogTrickDtos = new List<DogxTrickDto>();

            foreach (DogxTrick dogtrick in DogxTricks)
            {
                DogxTrickDto dogtrickdto = new DogxTrickDto();

                dogtrickdto.DogTrickId = dogtrick.DogTrickId;
                dogtrickdto.DogTrickDate = dogtrick.DogTrickDate;
                dogtrickdto.DogId = dogtrick.DogId;
                dogtrickdto.DogName = dogtrick.Dog.DogName;
                dogtrickdto.TrickId = dogtrick.TrickId;
                dogtrickdto.TrickName = dogtrick.Trick.TrickName;

                DogTrickDtos.Add(dogtrickdto);

            }


            return DogTrickDtos;
        }

        [HttpPost]
        [Route("api/DogxTrickData/AssociateTrickWithDog/{DogId}/{TrickId}")]
        public IHttpActionResult AssociateTrickWithDog(int DogId, int TrickId)
        {
            // Retrieve the selected dog and trick from the database
            Dog SelectedDog = db.Dogs.Where(d => d.DogId == DogId).FirstOrDefault();
            Trick SelectedTrick = db.Tricks.Find(TrickId);
            // Check if both the dog and trick exist
            if (SelectedDog == null || SelectedTrick == null)
            {
                return NotFound();
            }

            // Create a new DogxTrick entity
            var dogxTrick = new DogxTrick
            {
                DogId = DogId,
                TrickId = TrickId,
                DogTrickDate = DateTime.Now // Or any other date you want to set
            };

            // Add the new DogxTrick entity to the DbContext
            db.DogxTricks.Add(dogxTrick);


            db.SaveChanges();

            return Ok();
        }


        [HttpPost]
        [Route("api/DogxTrickData/UnAssociateTrickWithDog/{DogId}/{TrickId}")]
        public IHttpActionResult UnAssociateTrickWithDog(int DogId, int TrickId)
        {
            // Retrieve the existing association from the database
            var dogxTrick = db.DogxTricks
                .Where(dt => dt.DogId == DogId && dt.TrickId == TrickId)
                .FirstOrDefault();

            // Check if the association exists
            if (dogxTrick == null)
            {
                return NotFound(); // Return 404 Not Found if the association does not exist
            }

            // Remove the existing DogxTrick entity
            db.DogxTricks.Remove(dogxTrick);

            // Save changes to the database
            db.SaveChanges();

            return Ok(); // Return 200 OK if the operation was successful
        }


        // GET: api/DogxTrickData
        public IQueryable<DogxTrick> GetDogxTricks()
        {
            return db.DogxTricks;
        }

        // GET: api/DogxTrickData/5
        [ResponseType(typeof(DogxTrick))]
        public IHttpActionResult GetDogxTrick(int id)
        {
            DogxTrick dogxTrick = db.DogxTricks.Find(id);
            if (dogxTrick == null)
            {
                return NotFound();
            }

            return Ok(dogxTrick);
        }

        // PUT: api/DogxTrickData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDogxTrick(int id, DogxTrick dogxTrick)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dogxTrick.DogTrickId)
            {
                return BadRequest();
            }

            db.Entry(dogxTrick).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DogxTrickExists(id))
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

        // POST: api/DogxTrickData
        [ResponseType(typeof(DogxTrick))]
        public IHttpActionResult PostDogxTrick(DogxTrick dogxTrick)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DogxTricks.Add(dogxTrick);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dogxTrick.DogTrickId }, dogxTrick);
        }

        // DELETE: api/DogxTrickData/5
        [ResponseType(typeof(DogxTrick))]
        public IHttpActionResult DeleteDogxTrick(int id)
        {
            DogxTrick dogxTrick = db.DogxTricks.Find(id);
            if (dogxTrick == null)
            {
                return NotFound();
            }

            db.DogxTricks.Remove(dogxTrick);
            db.SaveChanges();

            return Ok(dogxTrick);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DogxTrickExists(int id)
        {
            return db.DogxTricks.Count(e => e.DogTrickId == id) > 0;
        }
    }
}
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
    public class DogDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all dogs in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all dogs in the database,
        /// </returns>
        /// <example>
        /// GET: api/DogData/ListDogs
        /// </example>
        [HttpGet]
        public IEnumerable<DogDto> ListDogs()
        {
            List<Dog> Dogs = db.Dogs.ToList();
            List<DogDto> DogDtos = new List<DogDto>();

            Dogs.ForEach(dog => DogDtos.Add(new DogDto()
            {
                DogId = dog.DogId,
                DogName = dog.DogName,
                DogAge = dog.DogAge,
                DogBreed = dog.DogBreed,
                DogBirthday = dog.DogBirthday,
                EmployeeId = dog.EmployeeId
            }));

            return DogDtos;
        }


        /// <summary>
        /// Returns one dog in the system by id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: one dog in the database,
        /// </returns>
        /// <example>
        /// GET: api/DogData/FindDog/5
        /// </example>

        [ResponseType(typeof(Dog))]
        [HttpGet]
        public IHttpActionResult FindDog(int id)
        {
            Dog dog = db.Dogs.Find(id);

            if (dog == null)
            {
                return NotFound();
            }
            DogDto DogDto = new DogDto()
            {
                DogId = dog.DogId,
                DogName = dog.DogName,
                DogAge = dog.DogAge,
                DogBreed = dog.DogBreed,
                DogBirthday = dog.DogBirthday,
                EmployeeId = dog.EmployeeId
            };
            return Ok(DogDto);
        }

        /// <summary>
        ///updatesone dog in the system by id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: update one dog in the database,
        /// </returns>
        /// <example>
        /// PUT: api/DogData/UpdateDog/5
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDog(int id, Dog dog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dog.DogId)
            {
                return BadRequest();
            }

            db.Entry(dog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DogExists(id))
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



        ///-----------------ListDogsForEmployee-------------------------------------------
        /// <summary>
        /// This method lists the number of dogs belongs to a particular employee in the database
        /// </summary>
        /// <param name="id">The id refers t the Employee Id </param>
        /// <returns>An array of dog objects</returns>
        /// <example>
        /// // GET: api/DogData/ListDogsForEmployee/3 =>
        // [{"DogId":1, "DogName":"Timbet", "DogAge":3, "DogBreed":"BullDog", "DogBirthday":"2021-03-03 12:00:00 AM", "EmployeeId":3},
        //  {"DogId":2, "DogName":"Mike", "DogAge":5, "DogBreed":"German Shephard", "DogBirthday":"2021-03-03 12:00:00 AM", "EmployeeId":3}]
        /// OR using command prompt 
        /// curl https://localhost:44351/api/DogData/ListDogsForEmployee/3
        /// [{"DogId":1, "DogName":"Timbet", "DogAge":3, "DogBreed":"BullDog", "DogBirthday":"2021-03-03 12:00:00 AM", "EmployeeId":3},
        //  {"DogId":2, "DogName":"Mike", "DogAge":5, "DogBreed":"German Shephard", "DogBirthday":"2021-03-03 12:00:00 AM", "EmployeeId":3}]
        /// </example>
        [System.Web.Http.HttpGet]
        public IEnumerable<DogDto> ListDogsForEmployee(int id)
        {

            List<Dog> Dogs = db.Dogs.Where(e => e.EmployeeId == id).ToList();
            List<DogDto> DogDtos = new List<DogDto>();

            foreach (Dog dog in Dogs)
            {
                DogDto dogDto = new DogDto();
                dogDto.DogId = dog.DogId;
                dogDto.DogName = dog.DogName;
                dogDto.DogBreed = dog.DogBreed;
                dogDto.DogAge= dog.DogAge;
                dogDto.DogBirthday = dog.DogBirthday;
                dogDto.EmployeeId = dog.EmployeeId;

                DogDtos.Add(dogDto);

            }




            return DogDtos;
        }

        // GET: api/DogData
        public IQueryable<Dog> GetDogs()
        {
            return db.Dogs;
        }

        // GET: api/DogData/5
        [ResponseType(typeof(Dog))]
        public IHttpActionResult GetDog(int id)
        {
            Dog dog = db.Dogs.Find(id);
            if (dog == null)
            {
                return NotFound();
            }

            return Ok(dog);
        }

        // PUT: api/DogData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDog(int id, Dog dog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dog.DogId)
            {
                return BadRequest();
            }

            db.Entry(dog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DogExists(id))
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

        // POST: api/DogData
        [ResponseType(typeof(Dog))]
        public IHttpActionResult PostDog(Dog dog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dogs.Add(dog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dog.DogId }, dog);
        }

        // DELETE: api/DogData/5
        [ResponseType(typeof(Dog))]
        public IHttpActionResult DeleteDog(int id)
        {
            Dog dog = db.Dogs.Find(id);
            if (dog == null)
            {
                return NotFound();
            }

            db.Dogs.Remove(dog);
            db.SaveChanges();

            return Ok(dog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DogExists(int id)
        {
            return db.Dogs.Count(e => e.DogId == id) > 0;
        }
    }
}
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
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// This method lists the number of Departments in the database
        /// </summary>
        /// <returns>An array of employee objects</returns>
        /// <example>
        /// GET: api/DepartmentData/ListDepartments =>
        /// [{"DepartmentId":1,"DepartmentName":"Inbound Stow","DepartmentManager":"Kim Min","NoOfEmployees":6},{"DepartmentId":3,"DepartmentName":"Outbound Pack","DepartmentManager":"Hani Patel Shah","NoOfEmployees":2},{ "DepartmentId":5,"DepartmentName":"Outbound Ship","DepartmentManager":"Tanvir Singh","NoOfEmployees":0}]
        /// OR using command prompt 
        ///curl https://localhost:44351/api/DepartmentData/ListDepartments
        ///[{"DepartmentId":1,"DepartmentName":"Inbound Stow","DepartmentManager":"Kim Min","NoOfEmployees":6},{"DepartmentId":3,"DepartmentName":"Outbound Pack","DepartmentManager":"Hani Patel Shah","NoOfEmployees":2},{ "DepartmentId":5,"DepartmentName":"Outbound Ship","DepartmentManager":"Tanvir Singh","NoOfEmployees":0}]
        /// </example>
        
        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        // GET: api/DepartmentData
        public IHttpActionResult ListDepartments()
        {
            //get the list of department
            List<Department> Departments = db.Departments.ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto>();

            Departments.ForEach(d => DepartmentDtos.Add(new DepartmentDto()
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                DepartmentManager = d.DepartmentManager,
                NoOfEmployees = d.NoOfEmployees
            }));

            return Ok(DepartmentDtos);
        }

        /// <summary>
        /// This method provides/fetch  the information about a particular department from the database
        /// </summary>
        /// <param name="id"> id refres to the DepartmentId of an Department whose information is requested</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A Department in the system matching up to the  Department ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example> 
        /// GET: api/DepartmentData/FindDepartment/5=> [{"DepartmentId":5,"DepartmentName":"Inbound Stow","DepartmentManager":"Kim Min","NoOfEmployees":6}]
        ///  OR using command prompt
        ///  curl https://localhost:44351/api/DepartmentData/FindDepartment/5 =>
        /// [{"DepartmentId":5,"DepartmentName":"Inbound Stow","DepartmentManager":"Kim Min","NoOfEmployees":6}]
        /// </example>

        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult FindDepartment(int id)
        {
            //find the department  wtih provided department id 
            Department department = db.Departments.Find(id);
            DepartmentDto departmentDto = new DepartmentDto()
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                DepartmentManager = department.DepartmentManager,
                NoOfEmployees = department.NoOfEmployees
            };
            if (department == null)
            {
                return NotFound();
            }

            return Ok(departmentDto);
        }
        // 
        /// <summary>
        /// This method updates the infomation about the current department in the database
        /// </summary>
        /// <param name="id"> The id of an department whose information needs to be updated</param>
        /// <param name="department">JSON FORM DATA of an Department </param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>>curl -d @department.json -H "Content-type:application/json"  https://localhost:44351/api/DepartmentData/UpdateDepartment/9 => updates the informatio of an department with DepartmentId =9 with the updated informtion listed in the department.json file
        /// POST: api/DepartmentData/UpdateDepartment/5
        /// FORM DATA:  Department JASON Object
        /// </example>

        [HttpPost]
        [ResponseType(typeof(void))]
        //[System.Web.Http.Authorize]
        public IHttpActionResult UpdateDepartment(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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
        // POST: api/DepartmentData/AddDepartment
        /// <summary>
        /// This method adds the new  department into the database
        /// </summary>
        /// <param name="department"> JSON FORM DATA of an Department</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Department ID, Department Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>>curl -d @department.json -H "Content-type:application/json"  https://localhost:44351/api/DepartmentData/AddDepartment => adds the new  Department object listed in the department.json file  into the database
        ///  POST: api/DepartmentData/AddDepartment
        /// FORM DATA: Department JSON Object
        /// </example>

        [HttpPost]
        [ResponseType(typeof(Department))]
       // [System.Web.Http.Authorize]
        public IHttpActionResult AddDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // add the new department into the database 
            db.Departments.Add(department);
            //save changes in the database 
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = department.DepartmentId }, department);
        }


        // POST: api/DepartmentData/DeleteDepartment/5
        /// <summary>
        /// This method deletes the specific department from the database by providing the id of an department as a parameter 
        /// </summary>
        /// <param name="id">The id of an department to be deleted</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example> Post: api/DepartmentData/DeleteDepartment/5  => deletes the department from the database having id = 5
        /// FORM DATA: (empty)
        /// curl -d ""  https://localhost:44351/api/DepartmentData/DeleteDepartment/5 =>deletes the department from the database having id = 8
        /// </example>
        //

        [HttpPost]
        [ResponseType(typeof(Department))]
        //[System.Web.Http.Authorize]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }
            // remove the deaprtment from the database 
            db.Departments.Remove(department);
            // save changes 
            db.SaveChanges();

            return Ok();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
        }
    }
}
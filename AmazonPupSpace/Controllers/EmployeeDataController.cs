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
using System.Web.Mvc;
using System.Web.Http.Description;
using AmazonPupSpace.Models;

namespace AmazonPupSpace.Controllers
{
    public class EmployeeDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        /// <summary>
        /// This method lists the number of employees in the database
        /// </summary>
        /// <returns>An array of employee objects</returns>
        /// <example>
        /// // GET: api/EmployeeData/ListEmployees =>
        /// [{"EmployeeId":6,"FirstName":"Amanpreet","LastName":"Kaur","HireDate":"0001-01-01T00:00:00"}]
        /// OR using command prompt 
        /// curl https://localhost:44351/api/EmployeeData/ListEmployees =>
        /// [{"EmployeeId":6,"FirstName":"Amanpreet","LastName":"Kaur","HireDate":"0001-01-01T00:00:00"}]
        /// </example>


        // GET: api/EmployeeData/ListEmployees
        [System.Web.Http.HttpGet]
        public IEnumerable<EmployeeDto> ListEmployees()
        {

            List<Employee> Employees = db.Employees.ToList();
            Debug.WriteLine(Employees);
            List<EmployeeDto> EmployeeDtos = new List<EmployeeDto>();

            foreach (Employee emp in Employees)
            {
                EmployeeDto empDto = new EmployeeDto();
                empDto.EmployeeId = emp.EmployeeId;
                empDto.FirstName = emp.FirstName;
                empDto.LastName = emp.LastName;
                empDto.HireDate = emp.HireDate;
                empDto.Email = emp.Email;
                empDto.DepartmentId = emp.Department.DepartmentId;
                empDto.DepartmentName= emp.Department.DepartmentName;
                empDto.PreviousDepartmentId = emp.PreviousDepartmentId;
                
                EmployeeDtos.Add(empDto);

            }
            return EmployeeDtos;
        }
        ///-----------------ListEmployeesForDepartment-------------------------------------------
        /// <summary>
        /// This method lists the number of employees belongs to a particular department in the database
        /// </summary>
        /// <param name="id">The id refers to the Department Id </param>
        /// <returns>An array of employee objects</returns>
        /// <example>
        /// // GET: api/EmployeeData/ListEmployeesForDepartment/3 =>
        // [{"EmployeeId":19,"FirstName":"Amanpreet","LastName":"Kaur","HireDate":"2024-05-28T04:00:00","DepartmentId":3,"DepartmentName":"Outbound Pack","PositionId":5,"PositionTitle":"L1-Warehouse Associate","PreviousDepartmentId":3,"PreviousPositionId":0},
        // {"EmployeeId":24,"FirstName":"Ranvir","LastName":"Singh","HireDate":"2024-05-28T04:00:00","DepartmentId":3,"DepartmentName":"Outbound Pack","PositionId":5,"PositionTitle":"L1-Warehouse Associate","PreviousDepartmentId":0,"PreviousPositionId":0}]
        /// OR using command prompt 
        /// curl https://localhost:44351/api/EmployeeData/ListEmployeesForDepartment/3
        ///[{"EmployeeId":19,"FirstName":"Amanpreet","LastName":"Kaur","HireDate":"2024-05-28T04:00:00","DepartmentId":3,"DepartmentName":"Outbound Pack","PositionId":5,"PositionTitle":"L1-Warehouse Associate","PreviousDepartmentId":3,"PreviousPositionId":0},
        /// {"EmployeeId":24,"FirstName":"Ranvir","LastName":"Singh","HireDate":"2024-05-28T04:00:00","DepartmentId":3,"DepartmentName":"Outbound Pack","PositionId":5,"PositionTitle":"L1-Warehouse Associate","PreviousDepartmentId":0,"PreviousPositionId":0}]
        /// </example>


        [System.Web.Http.HttpGet]
        public IEnumerable<EmployeeDto> ListEmployeesForDepartment(int id)
        {

            List<Employee> Employees = db.Employees.Where(e => e.DepartmentId == id).ToList();
            List<EmployeeDto> EmployeeDtos = new List<EmployeeDto>();

            foreach (Employee emp in Employees)
            {
                EmployeeDto empDto = new EmployeeDto();
                empDto.EmployeeId = emp.EmployeeId;
                empDto.FirstName = emp.FirstName;
                empDto.LastName = emp.LastName;
                empDto.HireDate = emp.HireDate;
                empDto.Email = emp.Email;
                empDto.DepartmentId = emp.Department.DepartmentId;
                empDto.DepartmentName = emp.Department.DepartmentName;
                empDto.PreviousDepartmentId = emp.PreviousDepartmentId;

                EmployeeDtos.Add(empDto);

            }




            return EmployeeDtos;
        }
        ///-----------------ListEmployeesNotInDepartment-------------------------------------------

        /// <summary>
        /// This method lists the number of employees who don't belongs to a particular department in the database
        /// </summary>
        /// <param name="id">The id refers to the Department Id </param>
        /// <returns>An array of employee objects</returns>
        /// <example>
        /// // GET: api/EmployeeData/ListEmployeesNotInDepartment/3 =>
        /// [{"EmployeeId":19,"FirstName":"Amanpreet","LastName":"Kaur","HireDate":"2024-05-28T04:00:00","DepartmentId":1,"DepartmentName":"Inbound Stow","PositionId":5,"PositionTitle":"L1-Warehouse Associate","PreviousDepartmentId":3,"PreviousPositionId":0},
        /// {"EmployeeId":24,"FirstName":"Ranvir","LastName":"Singh","HireDate":"2024-05-28T04:00:00","DepartmentId":1,"DepartmentName":"Inbound Stow","PositionId":5,"PositionTitle":"L1-Warehouse Associate","PreviousDepartmentId":0,"PreviousPositionId":0}]
        /// OR using command prompt 
        /// curl https://localhost:44351/api/EmployeeData/ListEmployeesNotInDepartment/3
        ///[{"EmployeeId":19,"FirstName":"Amanpreet","LastName":"Kaur","HireDate":"2024-05-28T04:00:00","DepartmentId":1,"DepartmentName":"Inbound Stow","PositionId":5,"PositionTitle":"L1-Warehouse Associate","PreviousDepartmentId":3,"PreviousPositionId":0},
        /// {"EmployeeId":24,"FirstName":"Ranvir","LastName":"Singh","HireDate":"2024-05-28T04:00:00","DepartmentId":1,"DepartmentName":"Inbound Stow","PositionId":5,"PositionTitle":"L1-Warehouse Associate","PreviousDepartmentId":0,"PreviousPositionId":0}]
        /// </example>



        [System.Web.Http.HttpGet]
        public IEnumerable<EmployeeDto> ListEmployeesNotInDepartment(int id)
        {

            List<Employee> Employees = db.Employees.Where(e => e.DepartmentId != id).ToList();
            List<EmployeeDto> EmployeeDtos = new List<EmployeeDto>();

            foreach (Employee emp in Employees)
            {
                EmployeeDto empDto = new EmployeeDto();
                empDto.EmployeeId = emp.EmployeeId;
                empDto.FirstName = emp.FirstName;
                empDto.LastName = emp.LastName;
                empDto.HireDate = emp.HireDate;
                empDto.Email = emp.Email;
                empDto.DepartmentId = emp.Department.DepartmentId;
                empDto.DepartmentName = emp.Department.DepartmentName;
               
                empDto.PreviousDepartmentId = emp.PreviousDepartmentId;
               

                EmployeeDtos.Add(empDto);

            }




            return EmployeeDtos;
        }


        //---------------------------------Associate  Department to an employer------------------------

        /// <summary>
        /// Changes the current Department of an employee to a new Department i.e changes the current department Id to a new department Id
        /// </summary>
        /// <param name="EmployeeId">The employee ID primary key</param>
        /// <param name="DepartmentId">The Department ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/EmployeeData/AssociateEmployeeToDepartment/9/1 => changes the current department Id of an employee (employee Id of 9 ) to a Department Id of 1 and returns the status of OK if  the Http request goes as intended
        /// </example>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/EmployeeData/AssociateEmployeeToDepartment/{EmployeeId}/{DepartmentId}")]
        //[System.Web.Http.Authorize]
        public IHttpActionResult AssociateEmployeeToDepartment(int EmployeeId, int DepartmentId)
        {
            Debug.WriteLine("EmployeeId" + EmployeeId);
            // Employee SelectedEmployee = db.Employees.Find(EmployeeId);
            // Department SelectedDepartment = db.Departments.Find(DepartmentId);
            Employee SelectedEmployee = db.Employees.Where(e => e.EmployeeId == EmployeeId).FirstOrDefault();
            Department SelectedDepartment = db.Departments.Where(d => d.DepartmentId == DepartmentId).FirstOrDefault();
            if (SelectedEmployee == null || SelectedDepartment == null)
            {
                return NotFound();
            }

            // Preserve the current Department id into the  PreviousDepartmentId column before changing it

            SelectedEmployee.PreviousDepartmentId = SelectedEmployee.DepartmentId;
            Debug.WriteLine("PreviousDepartmentId" + SelectedEmployee.PreviousDepartmentId);

            //changes the Department Id of an employee to the new Department Id
            SelectedEmployee.DepartmentId = SelectedDepartment.DepartmentId;

            //Save the changes into the database
            db.SaveChanges();

            return Ok();
        }


        //-----------------Remove Employer from a Department-----------------------------------------

        /// <summary>
        /// Changes the current Department of an employee to it's previous Department i.e changes the current department Id to it's previous department Id
        /// </summary>
        /// <param name="EmployeeId">The employee ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/EmployeeData/UnAssociateEmployeeToDepartment/9 => changes the current department Id of an employee (employee Id of 9 ) to it's Previous  Department Id  preserved in the PreviousDepartmentId Coulmn before editing the employee or
        /// before associating it to a new department and returns the status of OK if  the Http request goes as intended
        /// </example>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/EmployeeData/UnAssociateEmployeeToDepartment/{EmployeeId}")]
        //[System.Web.Http.Authorize]
        public IHttpActionResult UnAssociateEmployeeToDepartment(int EmployeeId)
        {
            Debug.WriteLine("EmployeeId" + EmployeeId);

            Employee SelectedEmployee = db.Employees.Where(e => e.EmployeeId == EmployeeId).FirstOrDefault();

            Debug.WriteLine("PreviousDepartmentId" + SelectedEmployee.PreviousDepartmentId);
            int departId = SelectedEmployee.PreviousDepartmentId;
            Debug.WriteLine("DepartId" + departId);
            Department SelectedDepartment = db.Departments.Where(d => d.DepartmentId == departId).FirstOrDefault();


            if (SelectedEmployee == null)
            {
                return NotFound();
            }

            //changes the current Department Id to it's previous Department Id 
            SelectedEmployee.DepartmentId = SelectedDepartment.DepartmentId;

            //Save the changes into the database
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// This method provides/fetch  the information about a particular employee from the database
        /// </summary>
        /// <param name="id"> id refres to the EmployeeId of an employee whose information is requested</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Employee in the system matching up to the  EmployeeId primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///  // GET: api/EmployeeData/FindEmployee/19=> [{"EmployeeId":19,"FirstName":"Amanpreet","LastName":"Kaur","HireDate":"2024-05-28T04:00:00","DepartmentId":3,"DepartmentName":"Outbound Pack","PositionId":6,"PositionTitle":"L2- Process Assistant","PreviousDepartmentId":3,"PreviousPositionId":0},
        ///  OR using command prompt
        ///  curl https://localhost:44351/api/EmployeeData/FindEmployee/19 =>
        ///  [{"EmployeeId":19,"FirstName":"Amanpreet","LastName":"Kaur","HireDate":"2024-05-28T04:00:00","DepartmentId":3,"DepartmentName":"Outbound Pack","PositionId":6,"PositionTitle":"L2- Process Assistant","PreviousDepartmentId":3,"PreviousPositionId":0},
        /// </example>


        [System.Web.Http.HttpGet]
        [ResponseType(typeof(EmployeeDto))]
        public IHttpActionResult FindEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            EmployeeDto employeeDto = new EmployeeDto()
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                HireDate = employee.HireDate,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department.DepartmentName,
                PreviousDepartmentId = employee.PreviousDepartmentId
            };
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employeeDto);
        }



        /// <summary>
        /// This method updates the infomation about the current employee in the database
        /// </summary>
        /// <param name="id"> The id of an employee whose information needs to be updated</param>
        /// <param name="employee">JSON FORM DATA of an Employee </param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>>curl -d @employee.json -H "Content-type:application/json"  https://localhost:44351/api/EmployeeData/UpdateEmployee/9 => updates the informatio of an employee with EmployeeId =9 with the updated informtion listed in the employee.json file
        /// POST: api/EmployeeData/UpdateEmployee/9
        /// FORM DATA: Employee JASON Object
        /// </example>
        [ResponseType(typeof(void))]
        [System.Web.Http.HttpPost]
        
        public IHttpActionResult UpdateEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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
        /// This method adds the new employee into the database
        /// </summary>
        /// <param name="employee"> JSON FORM DATA of an Employee</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Employee ID, Employee Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>>curl -d @employee.json -H "Content-type:application/json"  https://localhost:44351/api/EmployeeData/AddEmployee => adds the new  employee object listed in the employee.json file 
        /// POST: api/EmployeeData/AddEmployee
        /// FORM DATA: Employee JSON Object
        /// </example>
        [ResponseType(typeof(Employee))]
        [System.Web.Http.HttpPost]
        public IHttpActionResult AddEmployee(Employee employee)
        {
            Debug.WriteLine("Addemployeeaccessed");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // adds the employee into the database
            db.Employees.Add(employee);


            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employee.EmployeeId }, employee);
        }

        /// <summary>
        /// This method deletes the specific employee from the database by providing the id of an employee as a parameter 
        /// </summary>
        /// <param name="id">The id of an employee to be deleted</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example> Post: api/EmployeeData/DeleteEmployee/8  => deletes the employee from the database having id = 8
        /// FORM DATA: (empty)
        /// curl -d ""  https://localhost:44351/api/EmployeeData/DeleteEmployee/8 =>deletes the employee from the database having id = 8
        /// </example>
        //
        [ResponseType(typeof(Employee))]
        [System.Web.Http.HttpPost]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
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

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.EmployeeId == id) > 0;
        }
    }
}
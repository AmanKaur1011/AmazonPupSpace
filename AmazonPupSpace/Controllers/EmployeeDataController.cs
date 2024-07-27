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

        // GET: api/EmployeeData/ListEmployees
        [System.Web.Http.HttpGet]
        public IEnumerable<EmployeeDto> ListEmployees()
        {
            List<Employee> Employees = db.Employees.ToList();
            List<EmployeeDto> EmployeeDtos = new List<EmployeeDto>();
            foreach (Employee emp in Employees)
            {
                EmployeeDto empDto = new EmployeeDto();
                empDto.EmployeeId = emp.EmployeeId;
                empDto.FirstName = emp.FirstName;
                empDto.LastName = emp.LastName;
                empDto.HireDate = emp.HireDate;
                empDto.DepartmentId = emp.Department.DepartmentId;
                empDto.DepartmentName = emp.Department.DepartmentName;
                empDto.PreviousDepartmentId = emp.PreviousDepartmentId;
               

                EmployeeDtos.Add(empDto);

            }
            Debug.WriteLine(EmployeeDtos);

            return EmployeeDtos;
        }

        // GET: api/EmployeeData/5
        //[ResponseType(typeof(Employee))]
       // public IHttpActionResult GetEmployee(int id)
       // {
           // Employee employee = db.Employees.Find(id);
           // if (employee == null)
           // {
              //  return NotFound();
           // }

            //return Ok(employee);
       // }

        // PUT: api/EmployeeData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployee(int id, Employee employee)
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

        // POST: api/EmployeeData
        [ResponseType(typeof(Employee))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Employees.Add(employee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employee.EmployeeId }, employee);
        }

        // DELETE: api/EmployeeData/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
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
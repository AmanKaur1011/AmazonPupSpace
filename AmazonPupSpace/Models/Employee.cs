using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public int PreviousDepartmentId { get; set; }
        // Link to ASP.NET Identity User
        public string UserId { get; set; }

    }
    // Simplified version of the Employee Class to access data easily in the api and mvc controllers
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

       
        public int PreviousDepartmentId { get; set; }

    }

}
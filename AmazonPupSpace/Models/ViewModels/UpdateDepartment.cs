using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models.ViewModels
{
    // This View Model helps in  collecting data from two separate entities from two different models and to be used  as one object in different model 
    // Here we are using SelectedDepartment of type  IEnumerable<DepartmentDto> ,UnrealtedEmployees of type  IEnumerable<EmployeeDto> and  RelatedEmployees of type  IEnumerable<EmployeeDto>
    public class UpdateDepartment
    {
        public DepartmentDto SelectedDepartment { get; set; }

        public IEnumerable<EmployeeDto> RelatedEmployees { get; set; }

        public IEnumerable<EmployeeDto> UnrelatedEmployees { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models.ViewModels
{
    // This View Model helps in  collecting data from two separate entities from two different models and to be used  as one object in different model 
    // Here we are using SelectedEmployee of type  IEnumerable<EmployeeDto>  and DepartmentOPtions of type  IEnumerable<DepartmentDto> 
    public class UpdateEmployee
    {
        public EmployeeDto SelectedEmployee { get; set; }

        public IEnumerable<DepartmentDto> DepartmentOptions { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models.ViewModels
{

    // This View Model helps in  collecting data from two separate entities from two different models and to be used  as one object in different model 
    // Here we are using DepartmentDto and IEnumerable<EmployeeDto>
    public class DetailsDepartment
    {

        public DepartmentDto SelectedDepartment { get; set; }

        public IEnumerable<EmployeeDto> RelatedEmployees { get; set; }

    }
}
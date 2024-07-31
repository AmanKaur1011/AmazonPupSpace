using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models.ViewModels
{
    public class DetailsEmployee
    {

        public EmployeeDto SelectedEmployee { get; set; }

        public IEnumerable<DogDto> RelatedDogs { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models
{
    public class Dog
    {
        [Key]
        public int DogId { get; set; }
        public string DogName { get; set; }
        public int DogAge { get; set; }
        public string DogBreed { get; set; }
        public DateTime DogBirthday { get; set; }

        //a dog belongs to one user
        //a user can have many dogs(one for this MVP)
        [ForeignKey("Employee")]
        public int  EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        //a dog can learn many tricks
       // public ICollection<DogxTrick> Tricks { get; set; }

    }

    public class DogDto
    {
        public int DogId { get; set; }
        public string DogName { get; set; }
        public int DogAge { get; set; }
        public string DogBreed { get; set; }
        public DateTime DogBirthday { get; set; }
        public int EmployeeId { get; set; }
         public string FirstName { get; set; }
        public string LastName { get; set; }

    
}
}
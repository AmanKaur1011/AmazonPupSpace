using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models
{
    public class DogxTrick
    {
        [Key]
        public int DogTrickId { get; set; }

        public DateTime DogTrickDate { get; set; }

        //many dogs can learn many tricks
        //public ICollection<Dog> Dogs { get; set; }

        // public ICollection<Trick> Tricks { get; set; }

        // Foreign key for Dog
        [ForeignKey("Dog")]
        public int DogId { get; set; }
        public virtual Dog Dog { get; set; }

        // Foreign key for Trick
        [ForeignKey("Trick")]
        public int TrickId { get; set; }
        public  virtual Trick Trick { get; set; }
    }
    public class DogxTrickDto
    {
        public int DogTrickId { get; set; }

        public DateTime DogTrickDate { get; set; }
        public int DogId { get; set; }
         public  string DogName { get; set; }

        public int TrickId { get; set; }
         public string TrickName { get; set;}
    }
}
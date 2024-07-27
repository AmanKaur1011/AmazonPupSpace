using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmazonPupSpace.Models
{
    public class Trick
    {
        [Key]
        public int TrickId { get; set; }
        public string TrickName { get; set; }
        public string TrickVideoLink { get; set; }
        public string TrickDescription { get; set; }
        public string TrickDifficulty { get; set; }

        //a trick can be learned by many dogs
        public ICollection<DogxTrick> Dogs { get; set; }
    }
    public class TrickDto
    {
        public int TrickId { get; set; }
        public string TrickName { get; set; }
        public string TrickVideoLink { get; set; }
        public string TrickDescription { get; set; }
        public string TrickDifficulty { get; set; }
    }
}
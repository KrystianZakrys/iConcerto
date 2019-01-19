using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace iConcerto.Models
{

    public class UserData
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime CreateDate { get; set; }
        
        public virtual string ApplicationUserId { get; set; }

        public virtual ICollection<Events> Events { get; set; }
  
    }

    public class Events
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ImageURL { get; set; }
        
        public virtual int LocationId { get; set; }
        public virtual ICollection<UserData> Users { get; set; }
    }


    public class Locations
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }

        public virtual List<Events> Events { get; set; }

    }

}
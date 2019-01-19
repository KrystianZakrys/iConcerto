using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace iConcerto.Models
{

    public class UserData
    {
        [Key]
        public int UserDataId { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime CreateDate { get; set; }
        
        public virtual string ApplicationUserId { get; set; }

        public virtual ICollection<EventToUser> EventToUser { get; set; }
  
    }

    public class Events
    {
        [Key]
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ImageURL { get; set; }
        
        public virtual int LocationId { get; set; }
        public virtual ICollection<EventToUser> EventToUser { get; set; }
    }

    public class EventToUser
    {
        [Key, Column(Order =0)]
        public int UserDataId { get; set; }
        [Key, Column(Order = 1)]
        public int EventId { get; set; }

        public virtual UserData UserData { get; set; }
        public virtual Events Events { get; set; }

        public bool Notified { get; set; }
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
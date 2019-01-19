using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace iConcerto.Models
{
    public class EventsViewModel
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }


        public HttpPostedFileBase Files { get; set; }
        [Display(Name = "Location")]
        public string SelectedLocations { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Locations { get; set; }

    }

}
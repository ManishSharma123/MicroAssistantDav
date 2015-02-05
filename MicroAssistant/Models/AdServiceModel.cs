using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MicroAssistant.Data;
using MicroAssistant.Lib;

namespace MicroAssistant.Web.Models
{
    public class AdServiceModel
    {
        public string SELECT = "--- Select ---";
        public int Id { get; set; }
        [Required]
        [Display(Name = "What are you posting?")]
        public string Title { get; set; }


        public int CategoryId { get; set; }
        public IEnumerable<Category> Categories
        {
            get
            {
                return JobService.Instance.GetCategories();

            }
        }

        [AllowHtml]
        [Required]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Photo")]
        public HttpPostedFileBase Photo { get; set; }

        [Required]
        [Display(Name = "City")]

        public string City { get; set; }

        public int StateId { get; set; }

        public IEnumerable<State> States
        {
            get
            {
                return JobService.Instance.GetStates();

            }
        }
    }
}
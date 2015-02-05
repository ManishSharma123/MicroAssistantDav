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
    public class AdPostModel
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
        [Display(Name = "Description")]
        public string description { get; set; }
        
        [Display(Name = "Photo")]
        public HttpPostedFileBase Photo { get; set; }

       
        [Display(Name = "Video")]
        public string Video { get; set; }


        [Required]
        [Display(Name = "Price")]
        public Decimal Price { get; set; }

        [Required]
        [Display(Name = "BestOffer")]
        public bool IsBestOffer { get; set; }

        [Required]
        [Display(Name = "Free")]
        public bool IsFree { get; set; }
        
        [Required]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

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
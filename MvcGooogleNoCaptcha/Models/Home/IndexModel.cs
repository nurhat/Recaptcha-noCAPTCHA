using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcGooogleNoCaptcha.Models
{
    public class IndexModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blahgger.Models
{
    public class Blog
    {
        
        public int PostCreatorUserId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
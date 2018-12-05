using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }
        public string Role { get; set; }
        [Display(Name = "Can write messages")]
        public bool CanWriteMessages { get; set; }
        [Display(Name = "Can create room")]
        public bool CanCreateRoom { get; set; }

        [JsonIgnore]
        public virtual ICollection<Message> Messages { get; set; }
    }
}
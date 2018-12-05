using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
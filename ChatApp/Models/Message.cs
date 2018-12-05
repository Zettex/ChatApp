using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}
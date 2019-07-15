using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookSys.DAL.Models
{
    public class Book
    {
        [Key]
        public long ID { get; set; }
        public Guid MyGuid { get; set; }
        public string Title { get; set; }
        public int Copyright { get; set; }
    }
}

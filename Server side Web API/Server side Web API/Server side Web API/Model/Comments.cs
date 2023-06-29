using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A1.Model
{
    public class Comments
    {
        [Key]
        public int Id { get; set; }


        public string Name { get; set; }


        public DateTime Time { get; set; }


        public string Comment { get; set; }

        public string IP { get; set; }
    }
}

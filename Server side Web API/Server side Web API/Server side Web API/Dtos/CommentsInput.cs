using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A1.Dtos
{
    public class CommentsInput
    {
        public string Name { get; set; }

        public string Comment { get; set; }
    }
}

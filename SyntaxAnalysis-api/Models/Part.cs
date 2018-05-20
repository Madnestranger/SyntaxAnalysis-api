using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyntaxAnalysis_api.Models
{
    public class Part
    {
        [Key]
        public int id { get; set; }
        public string part { get; set; }
        public string com { get; set; }
    }
}

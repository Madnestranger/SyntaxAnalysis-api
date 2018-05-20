using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyntaxAnalysis_api.Models
{
    public class Nom
    {
        [Key]
        public string reestr { get; set; }
        public int type { get; set; }
        public int part { get; set; }
        public int field2 { get; set; }
        public int own { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string digit { get; set; }
    }
}

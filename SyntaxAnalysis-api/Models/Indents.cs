﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyntaxAnalysis_api.Models
{
    public class Indent
    {
        [Key]
        public int type { get; set; }
        public int indent { get; set; }
        public string comment { get; set; }
        public int? gr_id { get; set; }
    }
}

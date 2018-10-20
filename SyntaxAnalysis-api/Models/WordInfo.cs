using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyntaxAnalysis_api.Models
{
    public class WordInfo
    {
        public Nom nom { get; set; }
        public Indent indent { get; set; }
        public Flex flex { get; set; }
    }
}

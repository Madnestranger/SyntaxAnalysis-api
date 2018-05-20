using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SyntaxAnalysis_api.Models;

namespace SyntaxAnalysis_api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    public class ValuesController : Controller
    {
        private readonly DatabaseContext context = new DatabaseContext();
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { data = context.Flexes.First().digit });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public List<ResponseModel> Post([FromBody] TextModel textModel)
        {
            if (textModel == null)
            {
                throw new ArgumentNullException(nameof(textModel));
            }

            List<ResponseModel> info = new List<ResponseModel>();
            foreach (string word in textModel.Text)
            {
                Nom nom = GetNomEntity(word);
                if (nom != null)
                {
                    textModel.Text.Append(nom.reestr);
                }
            }
            return textModel.Text;
        }

        public Nom GetNomEntity(string word)
        {
            IQueryable<Nom> noms = context.Nom1.Where(x => x.reestr.Contains(word.ToLower()));
            if (noms == null)
            {
                return GetNomEntity(word.Remove(word.Length - 1));
            }
            else
            {
                foreach (Nom nom in noms)
                {
                    var newWord = nom.reestr.Remove(nom.reestr.Length - context.Indents.FirstOrDefault(x => x.type == nom.type).indent);
                    var res = GetAllPossibleFlexes(newWord, context.Flexes.Where(x => x.type == nom.type).ToList());
                }
            }
        }

        public string GetAllPossibleFlexes(string word, List<Flex> flexes)
        {
            string word_right = "";
            foreach(Flex flex in flexes)
            {
                var new_nom = CheckIfWordContainsInTable(word + flex.flex);
                if (new_nom != null)
                {
                    word_right = new_nom.reestr;
                }
            }
            return word_right;
        }

        public Nom CheckIfWordContainsInTable(string word)
        {
            return context.Nom1.FirstOrDefault(x => x.reestr == word.ToLower());
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

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
                    info.Add(new ResponseModel { part = context.Parts.FirstOrDefault(x => x.id == nom.part).com, comment = nom.field5});
                } else
                {
                    info.Add(new ResponseModel { });
                }
            }
            return info;
        }

        public Nom GetNomEntity(string word)
        {
            Nom result = null;
            IQueryable<Nom> noms = context.Nom1.Where(x => x.reestr.Contains(word.ToLower()));
            if (noms == null || noms.Count() == 0)
            {
                return GetNomEntity(word.Remove(word.Length - 1));
            }
            else
            {
                foreach (Nom nom in noms)
                {
                    Indents indentOfNewWord = context.Indents.FirstOrDefault(x => x.type == nom.@type);
                    IQueryable<Flex> flexes = context.Flexes.Where(x => x.type == nom.type);
                    string newWord = String.Empty;
                    if (indentOfNewWord.indent > 0)
                    {
                        newWord = nom.reestr.Remove(nom.reestr.Length - indentOfNewWord.indent);
                    } else
                    {
                        newWord = nom.reestr;
                    }
                    Nom res = GetAllPossibleFlexes(newWord, flexes.ToList(), word, nom.reestr);
                    if (res != null && res.reestr != null)
                    {
                        result = res;
                        break;
                    }
                }
            }
            if (result == null && word.Length > 1)
            {
                return GetNomEntity(word.Remove(word.Length - 1));
            }
            return result;
        }

        public Nom GetAllPossibleFlexes(string word, List<Flex> flexes, string result_word, string cur_nom_reestr)
        {
            Nom nom = new Nom();
            foreach(Flex flex in flexes)
            {
                var new_nom = word + flex.flex == result_word;
                if (new_nom == true)
                {
                    nom = GetNomByReestr(cur_nom_reestr);
                    break;
                }
            }
            return nom;
        }

        public Nom GetNomByReestr(string wordWithFlex)
        {
            return context.Nom1.FirstOrDefault(x => x.reestr == wordWithFlex);
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

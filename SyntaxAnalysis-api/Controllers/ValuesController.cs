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
                WordInfo wordInfo = GetNomEntity(word, new List<Nom>(), word);
                if (wordInfo != null && wordInfo.nom != null)
                {
                    var comment = ConstructTotalComment(wordInfo.nom);
                    var part_of_speech = GetPartOfSpeechText(wordInfo);
                    info.Add(new ResponseModel
                    {
                        part = context.Parts.FirstOrDefault(x => x.id == wordInfo.nom.part).com,
                        comment = comment,
                        part_of_speech = part_of_speech
                    });
                }
                else
                {
                    info.Add(new ResponseModel { });
                }
            }
            return info;
        }

        public WordInfo GetNomEntity(string word, List<Nom> checkedNoms, string fullWord)
        {
            WordInfo result = new WordInfo();
            IQueryable<Nom> noms = context.Nom1.Where(x => x.reestr.StartsWith(word.ToLower()));
            if (checkedNoms.Count() > 0)
            {
                noms = noms.Except(checkedNoms);
            }
            Nom potentialResult = noms.FirstOrDefault(x => x.reestr == fullWord);
            if (potentialResult != null)
            {
                result.nom = potentialResult;
                result.indent = context.Indents.FirstOrDefault(x => x.type == potentialResult.@type);
                return result;
            }
            if (noms == null || noms.Count() == 0)
            {
                return GetNomEntity(word.Remove(word.Length - 1), checkedNoms, fullWord);
            }
            else
            {
                foreach (Nom nom in noms)
                {
                    Indent indentOfNewWord = context.Indents.FirstOrDefault(x => x.type == nom.@type);
                    string newWord = String.Empty;
                    if (indentOfNewWord.indent > 0)
                    {
                        newWord = nom.reestr.Remove(nom.reestr.Length - indentOfNewWord.indent);
                    }
                    else
                    {
                        newWord = nom.reestr;
                    }
                    if (!word.StartsWith(newWord))
                    {
                        continue;
                    }
                    if (newWord == fullWord)
                    {
                        result.nom = nom;
                        result.indent = indentOfNewWord;
                        break;
                    }
                    IQueryable<Flex> flexes = context.Flexes.Where(x => x.type == nom.type && x.flex.Length == (fullWord.Length - newWord.Length));
                    NomFlex res = new NomFlex();
                    if (flexes.Count() > 0)
                    {
                        res = GetAllPossibleFlexes(newWord, flexes.ToList(), fullWord, nom.reestr);
                    }
                    if (res != null && res.nom != null && res.nom.reestr != null)
                    {
                        result.nom = res.nom;
                        result.indent = indentOfNewWord;
                        result.flex = res.flex;
                        continue;
                    }
                }
            }
            if (result.nom == null && word.Length > 1)
            {
                return GetNomEntity(word.Remove(word.Length - 1), noms.Concat(checkedNoms).ToList(), fullWord);
            }
            return result;
        }

        public NomFlex GetAllPossibleFlexes(string word, List<Flex> flexes, string result_word, string cur_nom_reestr)
        {
            NomFlex result = new NomFlex();
            foreach (Flex flex in flexes)
            {
                if ((word + flex.flex) == result_word)
                {
                    result.nom = GetNomByReestr(cur_nom_reestr);
                    result.flex = flex;
                    break;
                }
            }
            return result;
        }

        private string GetPartOfSpeechText(WordInfo info)
        {
            if (info != null)
            {
                if (info.flex == null && info.indent != null && info.nom != null)
                {
                    info.flex = context.Flexes.FirstOrDefault(x => x.type == info.nom.type && x.flex == info.nom.reestr.Substring(info.nom.reestr.Length - info.indent.indent));
                }
                if (info.flex != null)
                {
                    Gr gr = context.Gr.FirstOrDefault(x => x.id == info.indent.gr_id);
                    if (gr != null)
                    {
                        var grInfo = gr.GetType().GetProperty($"field{info.flex.field2 + 3}").GetValue(gr, null);
                        return grInfo.ToString();
                    }
                }
            }
            return "";
        }

        private string ConstructTotalComment(Nom nom)
        {
            var comment = "";
            if (nom.field5 != "" && nom.field5 != null)
            {
                comment += nom.field5;
                comment += ", ";
            }
            if (nom.field6 != "" && nom.field6 != null)
            {
                comment += nom.field6;
                comment += ", ";
            }
            if (nom.field7 != "" && nom.field7 != null)
            {
                comment += nom.field7;
                comment += ", ";
            }
            if (comment != "")
            {
                comment = comment.Remove(comment.Length - 2);
                comment += ".";
            }
            return comment;
        }

        public Nom GetNomByReestr(string wordWithFlex)
        {
            return context.Nom1.FirstOrDefault(x => x.reestr == wordWithFlex);
        }
    }
}

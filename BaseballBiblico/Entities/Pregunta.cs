using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseballBiblico.Entities
{
    public class Pregunta
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public string[] Answers { get; set; } = new string[4];
        public int CorrectAnswer { get; set; }


    }
}

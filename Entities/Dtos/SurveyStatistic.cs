using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class SurveyStatistic
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Count { get; set; }
    }
    public class QuestionStatistic
    {
        public string QuestionDescription { get; set; }
        public IEnumerable<AnswerStatistic> Answer { get; set; }
    }
    public class AnswerStatistic
    {
        public string Answer { get; set; }
        public int Count { get; set; }
    }
}

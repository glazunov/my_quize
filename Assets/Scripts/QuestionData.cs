using System.IO;

namespace MY.QUIZE
{
    [System.Serializable]
    public class QuestionData
    {
        public string theme;
        public string question;
        public string correctAnswer;
        public string[] wrongAnswers;
        public string explanation;
    }

}
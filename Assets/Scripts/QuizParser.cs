using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MY.QUIZE
{
    public static class QuizParser
    {
        public static List<QuestionData> ParseQuestions(string tsvFile)
        {
            var questions = new List<QuestionData>();

            foreach (string line in tsvFile.Split('\n'))
            {
                if (string.IsNullOrEmpty(line)) continue;

                string[] fields = line.Split('\t');
                questions.Add(new QuestionData
                {
                    theme = fields[0],
                    question = fields[1],
                    correctAnswer = fields[2],
                    wrongAnswers = fields.Skip(3).Take(3).ToArray(),
                    explanation = fields[6]
                });
            }
            return questions;
        }
    }

}
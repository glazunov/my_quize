using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace MY.QUIZE
{
    public class QuizManager : MonoBehaviour
    {
        // Serialized только префаб темы и панели
        [SerializeField] private Button themeButtonPrefab;
        [SerializeField] private Transform themesContainer;

        // Авто-найденные элементы UI
        [SerializeField] private TMP_Text question_text;
        [SerializeField] private TMP_Text explanation_text;
        [SerializeField] private Button[] answerButtons;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button skipButton;
        [SerializeField] private GameObject quizPanel;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private TextAsset questionsData;

        private List<QuestionData> currentQuestions;
        private int currentQuestionIndex;
        private int correctAnswerIndex;
        private ColorBlock defaultColors;
        private List<QuestionData> allQuestions;

        private void Awake()
        {
            nextButton.onClick.AddListener(ShowNextQuestion);
            backButton.onClick.AddListener(ReturnToMenu);
            skipButton.onClick.AddListener(SkipQuestion);

            foreach (var btn in answerButtons)
            {
                btn.onClick.AddListener(() => OnAnswerSelected(System.Array.IndexOf(answerButtons, btn)));
                btn.gameObject.SetActive(false);
            }

            defaultColors = answerButtons[0].colors;
        }



        private void Start()
        {
            allQuestions = QuizParser.ParseQuestions(questionsData);
            InitializeThemes(allQuestions);
            quizPanel.SetActive(false);
        }

        private void InitializeThemes(List<QuestionData> questions)
        {
            foreach (var theme in questions.Select(q => q.theme).Distinct())
            {
                Button themeButton = Instantiate(themeButtonPrefab, themesContainer);
                themeButton.gameObject.SetActive(true);
                themeButton.GetComponentInChildren<TMP_Text>().text = theme;
                themeButton.onClick.AddListener(() => StartQuiz(theme));
            }
        }
        private void StartQuiz(string theme)
        {
            currentQuestions = allQuestions.Where(q => q.theme == theme).ToList();
            currentQuestionIndex = -1;
            menuPanel.SetActive(false);
            quizPanel.SetActive(true);
            ShowNextQuestion();
        }

        private void ShowNextQuestion()
        {
            ResetAnswerButtons();
            currentQuestionIndex++;

            if (currentQuestionIndex >= currentQuestions.Count)
            {
                // Конец теста
                currentQuestionIndex = 0;
                return;
            }

            var question = currentQuestions[currentQuestionIndex];
            question_text.text = question.question;

            // Перемешивание ответов
            var answers = new List<string>(question.wrongAnswers) { question.correctAnswer };
            answers = answers.OrderBy(a => Random.value).ToList();
            correctAnswerIndex = answers.IndexOf(question.correctAnswer);

            // Назначение ответов на кнопки
            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TMP_Text>().text = answers[i];
                int index = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            }

            explanation_text.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
        }

        private void OnAnswerSelected(int selectedIndex)
        {
            foreach (var btn in answerButtons)
            {
                btn.interactable = false;
                var colors = btn.colors;
                colors.disabledColor = (btn == answerButtons[selectedIndex])
                    ? (selectedIndex == correctAnswerIndex ? Color.green : Color.red)
                    : colors.disabledColor;
                btn.colors = colors;
            }

            explanation_text.text = currentQuestions[currentQuestionIndex].explanation;
            explanation_text.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }

        private void ResetAnswerButtons()
        {
            foreach (var btn in answerButtons)
            {
                btn.gameObject.SetActive(false);
                btn.interactable = true;
                btn.colors = defaultColors;
            }
        }

        public void SkipQuestion()
        {
            ShowNextQuestion();
        }

        public void ReturnToMenu()
        {
            quizPanel.SetActive(false);
            menuPanel.SetActive(true);
            ResetAnswerButtons();
        }
    }

}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    [SerializeField] private Text previousResultText;
    [SerializeField] private Text targetText;
    [SerializeField] private Text scoreText;
    [SerializeField] private HandManager handManager;

    [Header("Game Settings")]
    [SerializeField] private int threshold = 10;
    [SerializeField] private int pointsMultiplier = 10;
    [SerializeField] private int drawEachRound = 3;
    [SerializeField] private int minRandRange = 2;
    [SerializeField] private int maxRandRange = 31;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Button saveButton;

    private int currentTarget;
    private int currentScore;

    private void Start()
    {
        saveButton.onClick.AddListener(SaveScoreToLeaderboard);
        gameOverPanel.SetActive(false);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeGame();
    }

    void InitializeGame()
    {
        currentScore = 0;
        GenerateNewTarget();
        UpdateTargetDisplay();
        UpdateScoreDisplay();
    }

    public void CheckGameOver()
    {
        bool isDeckEmpty = FindAnyObjectByType<DeckManager>().IsDeckEmpty();
        bool isHandEmpty = FindAnyObjectByType<HandManager>().IsHandEmpty();
        bool isHandFullOfOperators = IsHandFullOfOperators();
        
        if(isHandFullOfOperators || (isDeckEmpty && isHandEmpty))
        {
            gameOverPanel.SetActive(true);
        }
    }
    private bool IsHandFullOfOperators()
    {
        HandManager handManager = FindAnyObjectByType<HandManager>();
        if (handManager == null) return false;

        if (handManager.HandCards.Count != handManager.maxHandSize)
        {
            return false;
        }

        foreach (GameObject cardGO in handManager.HandCards)
        {
            Card card = cardGO.GetComponentInChildren<Card>();
            if (card == null || card.type != Card.CardType.Operator)
            {
                return false;
            }
        }
        
        return true;
    }
    public void ProcessResult(float result, string expression)
    {
        float difference = Mathf.Abs(currentTarget - result);
        bool isWithinThreshold = difference <= threshold;
        string comparisonDirection = currentTarget > result ? 
            $"{currentTarget} - {result:F2}" : 
            $"{result:F2} - {currentTarget}";

        int points = Mathf.FloorToInt((threshold - difference) * pointsMultiplier);
        currentScore += points;

        previousResultText.text = $"{expression} | {comparisonDirection} = " +
            $"{difference:F2} {(isWithinThreshold ? "<=" : ">")} {threshold} | " +
            $"{(points >= 0 ? "+" : "")}{points}";

        GenerateNewTarget();
        UpdateTargetDisplay();
        UpdateScoreDisplay();
        if (handManager != null)
        {   
            for(int i=0; i<drawEachRound; i++){
                handManager.DrawCard();
            } 
        }
        CheckGameOver();
    }

    void GenerateNewTarget()
    {
        currentTarget = Random.Range(minRandRange, maxRandRange);
    }

    void UpdateTargetDisplay()
    {
        targetText.text = $"Target: {currentTarget}";
    }

    void UpdateScoreDisplay()
    {
        scoreText.text = $"Score: {currentScore}";
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void SaveScoreToLeaderboard()
    {
        if(string.IsNullOrWhiteSpace(nameInputField.text))
        {
            nameInputField.text = "Anonymous";
        }
        
        Leaderboard.SaveScore(nameInputField.text, currentScore);
        gameOverPanel.SetActive(false);
        ReturnToMainMenu();
    }
}
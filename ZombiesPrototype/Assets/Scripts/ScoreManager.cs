using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Tooltip("Assign a UI Text element to display the score")]
    [SerializeField] private TMP_Text scoreText;

    public int Score { get; private set; }

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize display
        UpdateUI();
    }

    /// <summary>
    /// Call this to add points and immediately update the on-screen text.
    /// </summary>
    public void AddPoints(int amount)
    {
        Score += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + Score;
    }
}
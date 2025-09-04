using UnityEngine;
using TMPro;   // add this for TMP

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI deathText;

    [Header("Gameplay")]
    public int targetScore = 20;
    public Transform playerSpawnPoint;
    public GameObject player;

    private int score = 0;
    private int deaths = 0;

    void Start()
    {
        score = 0;
        deaths = 0;
        UpdateScoreUI();
        UpdateDeathUI();
        winText.gameObject.SetActive(false);
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreUI();

        if (score >= targetScore)
        {
            winText.gameObject.SetActive(true);
        }
    }

    public void PlayerDied()
    {
        deaths++;
        UpdateDeathUI();

        score = 0;
        UpdateScoreUI();

        ResetCollectibles();
        RespawnPlayer();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Gems: " + score;
    }

    private void UpdateDeathUI()
    {
        deathText.text = "Deaths: " + deaths;
    }

    private void RespawnPlayer()
    {
        if (player != null && playerSpawnPoint != null)
        {
            player.transform.position = playerSpawnPoint.position;

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    private void ResetCollectibles()
    {
        Collectable[] allCollectibles = FindObjectsOfType<Collectable>(true);
        foreach (Collectable c in allCollectibles)
        {
            c.gameObject.SetActive(true);
        }
    }
}

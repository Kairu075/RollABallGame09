using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") || other.CompareTag("Spike"))
        {
            gameManager.PlayerDied();
        }
    }
}

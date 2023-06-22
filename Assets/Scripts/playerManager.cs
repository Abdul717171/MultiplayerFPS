using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class playerManager : MonoBehaviour
{
    public TextMeshProUGUI health;
    public static int playerHP = 200;
    public static bool isGameOver;
    public GameObject playerBlood;
    void Start()
    {
        isGameOver = false;
        playerHP = 200;
    }

    // Update is called once per frame
    void Update()
    {
        health.text = "+" + playerHP;
        if (isGameOver)
        {
            //display game over scene
            SceneManager.LoadScene("Playground");
        }
    }

    public IEnumerator TakeDamage(int damageAmount)
    {
        playerBlood.SetActive(true);
        playerHP -= damageAmount;
        if (playerHP <= 0)
            isGameOver = true;

        yield return new WaitForSeconds(1.5f);
        playerBlood.SetActive(false);
    }
}

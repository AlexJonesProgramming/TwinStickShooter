using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    public int sceneNumber;
    public GameObject afterLevelUI;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SaveSystem.UpdateLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            SaveSystem.SaveGame();

            afterLevelUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

}

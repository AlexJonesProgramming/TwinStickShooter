using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameMaster : MonoBehaviour
{
    private PlayerMovement player;

    public GameObject afterDeathMenu;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (player.health <= 0)
        {
            afterDeathMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    IEnumerator ChangeLevel( float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene("AfterDeath");
    }

    public void Pause(GameObject pauseScreen)
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void Unpause(GameObject pauseScreen)
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

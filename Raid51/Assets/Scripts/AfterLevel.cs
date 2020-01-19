using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AfterLevel : MonoBehaviour
{

    private float timer;
   

    public Sprite[] sprites;
    public Image sprite;
    private int spriteIndex;

    private int unlockedCharacters;

    private bool allCharactersUnlocked;
    public TextMeshProUGUI banner;

    private Animator fadeAnimator;

    public GameObject logo;

    public bool afterDeath = false;

    // Start is called before the first frame update
    void Start()
    {
        SaveSystem.LoadGame();
        
        //fadeAnimator = GameObject.FindGameObjectWithTag("LevelFade").GetComponent<Animator>();

        if (!afterDeath)
        {
            unlockedCharacters = SaveSystem.unlockedCharacter;
            spriteIndex = unlockedCharacters;


            if (unlockedCharacters == 9)
            {
                allCharactersUnlocked = true;
                logo.SetActive(true);
                sprite.color = Color.clear;
                banner.text = "Watch an ad to support us!";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (afterDeath)
        {

        }
        else if (!allCharactersUnlocked)
        {
            timer += Time.deltaTime;

            if (timer > 2.5f)
            {
                spriteIndex += 1;

                if (spriteIndex > sprites.Length)
                    spriteIndex = unlockedCharacters;

                sprite.sprite = sprites[spriteIndex];

                timer = 0;
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        fadeTolevel();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            Destroy(player);
        Time.timeScale = 1;
        StartCoroutine(ChangeLevel(0));
    }

    public void Continue()
    {
        fadeTolevel();
        PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        player.ResetCheckpoints();
        Time.timeScale = 1;
        StartCoroutine(ChangeLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void UnlockCharacter()
    {
        SaveSystem.UpdateCharacter();
        SaveSystem.SaveGame();
    }

    public void fadeTolevel()
    {
        //fadeAnimator.SetTrigger("FadeOut");
    }

    IEnumerator ChangeLevel(int sceneNumber, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneNumber);
    }

    public void RestartLevel(int sceneNumber, float delay = 0)
    {
        fadeTolevel();
        PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        player.ResetCheckpoints();
        player.health = 110;

        StartCoroutine(ChangeLevel(SaveSystem.unlockedLevel, 1));
    }

    public void Respawn()
    {
        //fadeTolevel();
        PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        player.health = 100;
        player.Respawn();
        //StartCoroutine(ChangeLevel(player.respawnScene, 1));
    }

}

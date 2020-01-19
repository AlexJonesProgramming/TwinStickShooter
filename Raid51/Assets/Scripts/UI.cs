using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UI : MonoBehaviour
{
    public UnityEngine.UI.Button[] buttons;
    private int levelSelect = 0;

    public UnityEngine.UI.Button nextLevel;
    public UnityEngine.UI.Button lastLevel;
    public UnityEngine.UI.Text levelIndicator;

    private void Start()
    {
        SaveSystem.LoadGame();
        levelSelect = SaveSystem.unlockedLevel;
        levelIndicator.text = levelSelect.ToString();

        if (levelSelect == 1)
            lastLevel.interactable = false;
    }

    public void ChangeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelSelect);
    }

    public void CharacterSelect(GameObject GO)
    {
        GameObject.Instantiate(GO);
    }

    public void levelSelecter( bool negative = false)
    {
        if (negative)
            levelSelect -= 1;
        else
            levelSelect += 1;

        if (levelSelect == SaveSystem.unlockedLevel)
        {
            nextLevel.interactable = false;
            lastLevel.interactable = true;
        }
        else if (levelSelect == 1)
        {
            lastLevel.interactable = false;
            nextLevel.interactable = true;
        }
        else
        {
            nextLevel.interactable = true;
            lastLevel.interactable = true;
        }

        levelIndicator.text = levelSelect.ToString();

    }

    public void DisableCharacter()
    {
        int cap = SaveSystem.unlockedCharacter;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i > cap)
                buttons[i].interactable = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }
    public void VisitWebsite()
    {
        Application.OpenURL("https://www.facebook.com/events/rachel-nevada/storm-area-51-they-cant-stop-all-of-us/448435052621047/");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public static class SaveSystem
{
    public static int unlockedLevel = 1;
    public static int unlockedCharacter = 0;

    private static string path = Application.persistentDataPath + "data.data";
    public static void SaveGame()
    {
        StreamWriter stream = File.CreateText(path);
        stream.WriteLine(unlockedLevel.ToString());
        stream.WriteLine(unlockedCharacter.ToString());
        stream.Close();
    }

    public static void LoadGame()
    {
        if (System.IO.File.Exists(path))
        {
            StreamReader stream = new StreamReader(path);
            int.TryParse(stream.ReadLine(), out unlockedLevel);
            int.TryParse(stream.ReadLine(), out unlockedCharacter);
            stream.Close();
        }
        else
        {
            SaveGame();
            LoadGame();
        }
    }

    public static void UpdateLevel(int levelNumber)
    {
        if (levelNumber == unlockedLevel)
            unlockedLevel += 1;
    }

    public static void UpdateCharacter()
    {
        unlockedCharacter++;
        if (unlockedCharacter > 9)
            unlockedCharacter = 9;
    }
}

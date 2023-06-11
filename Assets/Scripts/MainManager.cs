using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static MainManager Instance;

    public Color TeamColor;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);    // This prevents multiple MainManager objects from being made. If Instance is already defined, destroy the duplicate
            return;
        }
        Instance = this;        // 'this' is the current Instance of MainManager. You can now call MainManager.Instance from any other script and get a link to that specific instance of it
        DontDestroyOnLoad(gameObject);      // marks the MainManager GameObject attatched to this script not to be destroyed when the scene changes

        LoadColor();
    }

    // This code above enables you to access the MainManager from any other script

    [System.Serializable]       // SaveData is a simple class, which only contains teh color that the user selects. The [System.Seralizable] is required for JsonUtility, as it will only transform things to JSON if they are tagged as Serializable
    class SaveData
    {
        public Color TeamColor;
    }
    // Why are we creating a whole new class SaveData and not giving MainManager instance directly to JsonUtility? It's good practice, and most of the time you won't save everything inside your classes
    // It's more efficient to use a small class that only contains the specific data you want to save

    public void SaveColor()
    {
        SaveData data = new SaveData();     // First, you create a new instance of the save data and filled its team color class member with the TeamColor variable saved in the MainManager
        data.TeamColor = TeamColor;     // Next, you transformed that instance to JSON with JsonUtility.ToJson

        string json = JsonUtility.ToJson(data);     // Next, you transformed that instance to JSON with JsonUtility.ToJson 
    
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);     // Finally, you used the special method File.WriteAllText to write a string to a file
        // The first parameter is the path to the file-- Application.persistentDataPath is a Unity method that will give you a folder where you can save data that will survive
        // between application reinstal or update and append it to the filename savefile.json. The second parameter is the text you want to right to the file, aka your JSON data
    }

    public void LoadColor()     // This method is a reversal of the SaveColor method
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))      // Check if file exists with file name string path
        {
            string json = File.ReadAllText(path);       // If it exists, write all text to a string
            SaveData data = JsonUtility.FromJson<SaveData>(json);       // Give resulting text to JsonUtility.FromJson to transform it back into a SaveData instance

            TeamColor = data.TeamColor;     // Finally, set TeamColor to the color saved in that SaveData
        }
    }
}

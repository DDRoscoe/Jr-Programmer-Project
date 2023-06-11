using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR        // like other namespaces, this is needed for the #if #else #endif things in Exit()
using UnityEditor;
#endif


// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public ColorPicker ColorPicker;

    public void NewColorSelected(Color color)
    {
        MainManager.Instance.TeamColor = color;     // The color that the user selects is now stored in MainManager, so you
                                                    // can access the TeamColor variable from the Main scene to retrieve it
    }
    
    private void Start()
    {
        ColorPicker.Init();     // This will call the NewColorSelected function when the color picker have a color button clicked.
        ColorPicker.onColorChanged += NewColorSelected;

        ColorPicker.SelectColor(MainManager.Instance.TeamColor);    // This line will pre-select the saved color in the MainManager (if there is one) when the menu is launched
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR        // These if/else statements with hashtags are instructions for the compiler. It's the same as: if (Unity_EDITOR) {} else {}
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // Original code to quit Unity player
#endif

        MainManager.Instance.SaveColor(); 
    }

    public void SaveColorClicked()
    {
        MainManager.Instance.SaveColor();
    }

    public void LoadColorClicked()
    {
        MainManager.Instance.LoadColor();
        ColorPicker.SelectColor(MainManager.Instance.TeamColor);
    }
}

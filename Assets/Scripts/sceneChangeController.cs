using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChangeController : MonoBehaviour
{
    private string[] sceneName = new string[4];

    void Start()
    {
        sceneName[0] = "Room_main";
        sceneName[1] = "Room_dylan";
        sceneName[2] = "Room_lucy";
        sceneName[3] = "Room_jack";
    }

    void Update()
    {
        ChangeScene();
    }

    void ChangeScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        switch (sensorTrigger.currentPage)
        {
            case 1:
                if (currentScene.name != sceneName[0]) SceneManager.LoadScene(sceneName[0]);
                break;

            case 2:
                if (currentScene.name != sceneName[1]) SceneManager.LoadScene(sceneName[1]);
                break;

            case 3:
                if (currentScene.name != sceneName[2]) SceneManager.LoadScene(sceneName[2]);
                break;

            case 4:
                if (currentScene.name != sceneName[3]) SceneManager.LoadScene(sceneName[3]);
                break;

            default:
                if (currentScene.name != "Title") SceneManager.LoadScene("Title");
                break;
        }
    }
}

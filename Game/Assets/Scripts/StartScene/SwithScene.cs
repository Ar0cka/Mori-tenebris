using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwithScene : MonoBehaviour
{
    [SerializeField] private string nameScene;

    public void NextScene()
    {
        SceneManager.LoadScene(nameScene);
    }
}

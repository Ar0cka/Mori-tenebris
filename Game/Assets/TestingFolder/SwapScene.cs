using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapScene : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    public void NextScene()
    {
        SceneManager.LoadScene(_sceneName);
    }
}

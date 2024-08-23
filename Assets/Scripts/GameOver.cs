using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public void OnTryAgainButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnReturnToMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
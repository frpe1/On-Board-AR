using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QRCode ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Changed scene");
    }

    public void Entré ()
    {
        Debug.Log("Entré Selected");
    }
    public void Klassrum()
    {
        Debug.Log("Klassrum Selected");
    }
    public void Expeditionen()
    {
        Debug.Log("Expeditionen Selected");
    }
}

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

    // Obs. jag lade till LoadScene under varje alternativ för att testa knapparna på android /Karl
    public void Entré ()
    {
        Debug.Log("Entré Selected");
        SceneManager.LoadScene("Game");
    }
    public void Klassrum()
    {
        Debug.Log("Klassrum Selected");
        SceneManager.LoadScene("Menu");
    }
    public void Expeditionen()
    {
        Debug.Log("Expeditionen Selected");
        SceneManager.LoadScene("KarlTestScene");
    }
}

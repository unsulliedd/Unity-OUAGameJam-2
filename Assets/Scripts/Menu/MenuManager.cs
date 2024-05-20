using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("PrototypeScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class swichLevel : MonoBehaviour {

    public void Level0()
    {
        SceneManager.LoadScene(0);
    }

    public void Level1()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

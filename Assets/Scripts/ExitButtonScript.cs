using UnityEngine;

public class ExitButtonScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void exitGame()
    {
        Debug.Log("exitting");
        Application.Quit();
    }

}


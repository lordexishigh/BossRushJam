using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void RestartScene()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public static void NextScene()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static int GetActiveSceneIndex()
	{
        return SceneManager.GetActiveScene().buildIndex;
    }
}

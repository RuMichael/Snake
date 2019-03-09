using UnityEngine;
using UnityEngine.SceneManagement;

public class OverClick : MonoBehaviour
{
    void Start()
    {
        
    }


    public void ClickPlay()
    {
        SceneManager.LoadScene("SnakeScene");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class ret : MonoBehaviour
{
    public void PlayAgain(){
        SceneManager.LoadScene("MainMenu");
    }
}

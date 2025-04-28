using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenSwitcher : MonoBehaviour
{
    [SerializeField] private AudioClip gameOverClip;

    void Start() {
        FindObjectOfType<AudioSource>().Pause();
        AudioSource.PlayClipAtPoint(gameOverClip, Camera.main.transform.position);
    }

    public void restartGame()
    {
        SceneManager.LoadScene("SampleScene");
        Debug.Log("Restart");
    }

    public void goToMenu()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Menu");
    }

    void OnDestroy() {
        FindObjectOfType<AudioSource>().UnPause();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIScript : MonoBehaviour
{
	[SerializeField]
	private string sceneName;

	public void loadScene(){
		SceneManager.LoadScene(sceneName);	
	}

	public void Start() {
		FindFirstObjectByType<AudioSource>().UnPause();
	}
}

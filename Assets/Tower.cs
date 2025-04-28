using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class Tower : MonoBehaviour
{
    [SerializeField] private Transform towerBase;
    [SerializeField] private float maxTiltAngle = 15f;
    [SerializeField] private string gameOverSceneName = "Game Over";
    [SerializeField] private float slowMotionScale = 0.2f;

    private bool triggered = false;

    void Update()
    {
        if (!triggered)
        {
            float tiltAngle = Vector3.Angle(Vector3.up, towerBase.up);
            if (tiltAngle > maxTiltAngle)
            {
                triggered = true;
                Time.timeScale = slowMotionScale;
                Debug.Log("Swayed too much! Waiting for touch...");
            }
        }
        else if (Input.touchCount > 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(gameOverSceneName);
        }
    }

    public void SetTowerBase(Transform baseTransform)
    {
        towerBase = baseTransform;
    }
}

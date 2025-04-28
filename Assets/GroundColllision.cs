using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class GroundColllision : MonoBehaviour
{

    private bool fallen = false;

    private void Update()
    {
        if (fallen && (Input.touchCount > 0))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Game Over");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!fallen && collision.gameObject.CompareTag("Ground"))
        {
            fallen = true;
            gameObject.tag = "Fallen Block";
            Time.timeScale = 0.2f;
            Debug.Log("Collided with ground! Waiting for touch...");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Rigidbody otherBody = collision.rigidbody;

            // Avoid duplicate joints
            bool alreadyConnected = false;
            foreach (FixedJoint joint in GetComponents<FixedJoint>())
            {
                if (joint.connectedBody == otherBody)
                {
                    alreadyConnected = true;
                    break;
                }
            }

            if (!alreadyConnected)
            {
                StartCoroutine(AttachJoint(otherBody));
            }
        }
    }


    IEnumerator AttachJoint(Rigidbody target)
    {
        yield return new WaitForFixedUpdate();

        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = target;
        joint.breakTorque = 5f;
        joint.enableCollision = false;
    }

}

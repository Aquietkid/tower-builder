using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private float highestY = 0f;

    public void CheckNewBlock(GameObject block)
    {
        float blockY = block.transform.position.y;
        if (blockY > highestY)
        {
            highestY = blockY;
            scoreText.text = $"Score: {Mathf.FloorToInt(highestY)}";
        }
    }
}

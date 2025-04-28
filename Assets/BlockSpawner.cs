using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class BlockSpawner : MonoBehaviour
{
	[SerializeField] private GameObject[] blockPrefabs; // The blocks to be spawned randomly
	[SerializeField] private Camera mainCamera; // The main camera of the scene
	[SerializeField] private Transform cameraTarget; // 
	[SerializeField] private float cameraMoveSpeed = 5f;
	[SerializeField] private Button moveCameraUpButton; // UI button to move camera upwards
	[SerializeField] private Button moveCameraDownButton; // UI button to move camara downwards
	[SerializeField] private float baseWindStrength = 0.01f; // The base wind strength at ground level (Wind speed increases with height)
	[SerializeField] private Vector3 windDirection = Vector3.left; // The direction in which the wind is blown
	[SerializeField] private Color[] colorPalette; // The color palette for randomized assignment to spawned blocks
	private float manualYOffset = 10f;
	private float currentMaxBlockY = 0f;
	private GameObject nextBlock;
	private bool baseAssigned = false;

	void OnEnable()
	{
		moveCameraUpButton.onClick.AddListener(() => manualYOffset += 1f);
		moveCameraDownButton.onClick.AddListener(() => manualYOffset -= 1f);
	}

	void Start()
	{
		FindObjectOfType<AudioSource>().UnPause();
		SetNextBlock();
	}

	void Update()
	{
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
			TrySpawnAt(Input.GetTouch(0).position);
		else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
			TrySpawnAt(Input.mousePosition);
	}

	void FixedUpdate()
	{
		GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

		foreach (GameObject block in blocks)
		{
			Rigidbody rb = block.GetComponent<Rigidbody>();
			if (rb == null) continue;

			float heightFactor = Mathf.Clamp01(block.transform.position.y / 50f);
			Vector3 windForce = windDirection * baseWindStrength * heightFactor;
			rb.AddForce(windForce, ForceMode.Force);
		}
	}

	void LateUpdate()
	{
		float targetY = currentMaxBlockY + manualYOffset;
		Vector3 targetPos = new Vector3(cameraTarget.position.x, targetY, cameraTarget.position.z);
		cameraTarget.position = Vector3.Lerp(cameraTarget.position, new Vector3(targetPos.x, Mathf.Max(targetPos.y, 5f), targetPos.z), Time.deltaTime * cameraMoveSpeed);
	}

	void TrySpawnAt(Vector2 screenPos)
	{
		Ray ray = mainCamera.ScreenPointToRay(screenPos);
		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			Vector3 spawnPos = hit.point;
			float blockHeight = nextBlock.GetComponent<Renderer>().bounds.size.y;
			spawnPos.y = hit.collider.bounds.max.y + blockHeight + 0.2f;

			if (spawnPos.y > currentMaxBlockY)
			{
				currentMaxBlockY = spawnPos.y;
			}

			GameObject spawned = Instantiate(nextBlock, spawnPos, Quaternion.identity);

			var renderer = spawned.GetComponent<Renderer>();
			if (renderer != null && colorPalette.Length > 0)
			{
				renderer.material.color = colorPalette[Random.Range(0, colorPalette.Length)];
			}

			FindFirstObjectByType<ScoreManager>().CheckNewBlock(spawned);

			if (!baseAssigned)
			{
				FindFirstObjectByType<Tower>().SetTowerBase(spawned.transform);
				baseAssigned = true;
			}

			SetNextBlock();
		}
	}

	void SetNextBlock()
	{

		int index = Random.Range(0, blockPrefabs.Length);
		// int index = 3;

		nextBlock = blockPrefabs[index];

	}

}

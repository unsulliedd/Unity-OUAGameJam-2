using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBuilding : MonoBehaviour
{
    private Player player;
    [SerializeField] private GameObject[] buildingPrefabs;
    [SerializeField] private LayerMask buildableLayerMask;
    [SerializeField] private LayerMask buildingLayerMask;
    [SerializeField] private float previewHeight = 0.5f;
    [SerializeField] private float maxSnapDistance = 1f;
    private int buildingIndex = -1;
    private GameObject buildingPreviewInstance;
    private UIManager uiManager;

    void Awake()
    {
        player = GetComponent<Player>();
        uiManager = FindObjectOfType<UIManager>();

        player.controls.Player.BuildMenu.performed += ctx => OpenBuildMenu();
        player.controls.Building.Build.performed += ctx => PlaceBuilding();
        player.controls.Building.Rotate.performed += ctx => RotatePreview();
    }

    void Update()
    {
        if (buildingPreviewInstance != null)
            UpdateBuildingPreviewPosition();
    }

    public void SetBuildingIndex(int index)
    {
        buildingIndex = index;
        if (buildingPreviewInstance != null)
            Destroy(buildingPreviewInstance);

        if (buildingIndex >= 0 && buildingIndex < buildingPrefabs.Length)
        {
            buildingPreviewInstance = Instantiate(buildingPrefabs[buildingIndex]);
            SetPreviewMode(buildingPreviewInstance, true);
        }
    }

    private void UpdateBuildingPreviewPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildableLayerMask))
        {
            Vector3 buildPosition = hit.point;
            buildPosition.y = hit.point.y;  // Snap to ground level
            buildPosition.y += previewHeight;

            bool isNearAnotherBuilding = CheckNearbyBuildings(buildPosition);
            if (isNearAnotherBuilding)
            {
                // Snap to nearby building
                Vector3 snapPosition = FindSnapPosition(buildPosition);
                buildingPreviewInstance.transform.position = snapPosition;
            }
            else
                buildingPreviewInstance.transform.position = buildPosition;
        }
    }

    public void RotatePreview()
    {
        if (buildingPreviewInstance != null)
            buildingPreviewInstance.transform.Rotate(Vector3.up, 90f);
    }

    private bool CheckNearbyBuildings(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, maxSnapDistance, buildingLayerMask);
        return colliders.Length > 0;
    }

    private Vector3 FindSnapPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, maxSnapDistance, buildingLayerMask);
        Vector3 closestPoint = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            Vector3 pointOnCollider = collider.ClosestPoint(position);
            float distance = Vector3.Distance(position, pointOnCollider);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = pointOnCollider;
            }
        }

        return closestPoint;
    }

    private void PlaceBuilding()
    {
        if (buildingPreviewInstance != null)
        {
            Transform obstacleParent = GameObject.Find("Obstacle").transform;

            Instantiate(buildingPrefabs[buildingIndex], buildingPreviewInstance.transform.position, buildingPreviewInstance.transform.rotation, obstacleParent);
            Destroy(buildingPreviewInstance);
            buildingPreviewInstance = null;
            buildingIndex = -1;
        }
    }

    private void SetPreviewMode(GameObject building, bool isPreview)
    {
        Collider[] colliders = building.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            if (isPreview)
            {
                collider.gameObject.layer = LayerMask.NameToLayer("Preview");
            }
            else
            {
                collider.gameObject.layer = LayerMask.NameToLayer("Obstacles");
            }
        }
    }

    private void OpenBuildMenu()
    {
        uiManager.OpenBuildMenu();
    }

    public void ExitBuildMode()
    {
        if (buildingPreviewInstance != null)
        {
            Destroy(buildingPreviewInstance);
        }
        buildingIndex = -1;
    }
}
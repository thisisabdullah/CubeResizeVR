using TMPro;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class AutomaticCubeDetector : MonoBehaviour
{
    [SerializeField] private GameObject rubixCubePrefab;
    [SerializeField] private GameObject UiGameobject;
    public MRUKAnchor.SceneLabels sceneLabels;
    [SerializeField] private Transform rayStartPoint;
    [SerializeField] private float rayLength;
    public OVRHand LeftHand;

    private bool isCubePlaced = false;
    private bool isObjectPlaced = false;
    //[SerializeField] private float desiredHeightOffset;

    private void Update()
    {
        // Check if the index finger is pinching
        if (LeftHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            isCubePlaced = !isCubePlaced;
        }

        // Prevent continuous placement if the cube is already placed
        if (isCubePlaced) return;
        if (isObjectPlaced) return;

        // Create a ray from the specified start point
        Ray ray = new Ray(rayStartPoint.position, rayStartPoint.forward);
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();

        if (room != null)
        {
            // Perform the raycast
            bool hasHit = room.Raycast(ray, rayLength, LabelFilter.FromEnum(sceneLabels), out RaycastHit hit, out MRUKAnchor anchor);

            if (hasHit && anchor != null)
            {
                // Get the center of the table anchor
                Vector3 tableCenter = anchor.GetAnchorCenter();

                // Adjust the Y position to place the cube on the surface
                Vector3 surfacePosition = new Vector3(tableCenter.x, hit.point.y, tableCenter.z);

                rubixCubePrefab.SetActive(true);
                //UiGameobject.SetActive(true);

                // Place the cube at the calculated surface position
                rubixCubePrefab.transform.position = surfacePosition+ Vector3.down * 0.03f; // Add a small offset if needed
                rubixCubePrefab.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                isObjectPlaced = true;
            }

        }
    }
}
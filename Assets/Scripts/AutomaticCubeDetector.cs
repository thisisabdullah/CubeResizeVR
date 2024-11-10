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

        // Create a ray from the specified start point
        Ray ray = new Ray(rayStartPoint.position, rayStartPoint.forward);
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();

        if (room != null)
        {
            // Perform the raycast
            bool hasHit = room.Raycast(ray, rayLength, LabelFilter.FromEnum(sceneLabels), out RaycastHit hit, out MRUKAnchor anchor);

            if (hasHit )
            {
                rubixCubePrefab.SetActive(true);
                UiGameobject.SetActive(true);
                rubixCubePrefab.transform.position = hit.point + Vector3.up * 0.03f;
                rubixCubePrefab.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }
}
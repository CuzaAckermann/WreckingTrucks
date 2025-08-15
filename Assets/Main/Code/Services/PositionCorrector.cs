using UnityEngine;

public class PositionCorrector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rayLength;
    [SerializeField] private LayerMask _layerMask;

    private const float MiddleOfModel = 0.5f;

    public void CorrectTransformable(Transform fieldPosition, TruckSpaceSettings truckSpaceSettings)
    {
        float halfWidthOfScreen = Screen.width / 2f;

        Ray ray = _camera.ScreenPointToRay(new Vector3(halfWidthOfScreen, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, _rayLength, _layerMask))
        {
            fieldPosition.forward = -Vector3.ProjectOnPlane(_camera.transform.forward, hit.normal).normalized;
            fieldPosition.position = new Vector3(hit.point.x,
                                                 fieldPosition.position.y,
                                                 hit.point.z);
            fieldPosition.position += GetOffset(fieldPosition, truckSpaceSettings);
        }
    }

    private Vector3 GetOffset(Transform fieldPosition,
                              TruckSpaceSettings truckSpaceSettings)
    {
        float halfOfField = truckSpaceSettings.FieldSettings.FieldSize.AmountColumns / 2f;

        float offsetX = (halfOfField - MiddleOfModel) * truckSpaceSettings.FieldIntervals.BetweenColumns;
        float offsetZ = truckSpaceSettings.FieldIntervals.BetweenRows;

        return -fieldPosition.right * offsetX - fieldPosition.forward * offsetZ;
    }
}
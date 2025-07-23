using UnityEngine;

public class PositionCorrector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rayLength = 30f;
    [SerializeField] private LayerMask _layerMask;

    private const float MiddleOfModel = 0.5f;

    public void CorrectTransformable(Transform fieldPosition, FieldSize fieldSize, FieldIntervals fieldIntervals)
    {
        float halfWidthOfScreen = Screen.width / 2f;

        Ray ray = _camera.ScreenPointToRay(new Vector3(halfWidthOfScreen, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, _rayLength, _layerMask, QueryTriggerInteraction.Ignore))
        {
            fieldPosition.forward = -Vector3.ProjectOnPlane(_camera.transform.forward, hit.normal).normalized;

            fieldPosition.position += GetOffset(fieldPosition, fieldSize, fieldIntervals);
        }
    }

    private Vector3 GetOffset(Transform fieldPosition,
                              FieldSize fieldSize,
                              FieldIntervals fieldIntervals)
    {
        float halfOfField = fieldSize.AmountColumns / 2f;

        float offsetX = (halfOfField - MiddleOfModel) * fieldIntervals.BetweenColumns;

        float offsetZ = MiddleOfModel * fieldIntervals.BetweenRows;

        return -fieldPosition.right * offsetX - fieldPosition.forward * offsetZ;
    }
}
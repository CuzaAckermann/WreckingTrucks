using UnityEngine;

public class PositionCorrector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rayLength = 30f;
    [SerializeField] private LayerMask _layerMask;

    private const float MiddleOfModel = 0.5f;

    private Vector3 _middleScreenPos = new Vector3(1920 / 2f, 0f);

    public void CorrectTransformable(Transform transform, FieldSettings fieldSettings)
    {
        Ray ray = _camera.ScreenPointToRay(_middleScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayLength, _layerMask, QueryTriggerInteraction.Ignore))
        {
            transform.forward = -Vector3.ProjectOnPlane(_camera.transform.forward, hit.normal).normalized;

            float middleOfField = fieldSettings.FieldSize.AmountColumns / 2f;

            float offsetX = (middleOfField - MiddleOfModel) * fieldSettings.FieldSize.IntervalBetweenColumns;

            Vector3 offsetPosition = -transform.right * offsetX - transform.forward * fieldSettings.FieldSize.IntervalBetweenRows;

            transform.position += offsetPosition;
        }
    }
}
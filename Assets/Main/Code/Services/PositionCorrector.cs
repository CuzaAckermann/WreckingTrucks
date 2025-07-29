using System;
using UnityEngine;

public class PositionCorrector
{
    private const float MiddleOfModel = 0.5f;

    private readonly float _rayLength;
    private readonly Camera _camera;

    public PositionCorrector(Camera camera, float rayLength)
    {
        if (rayLength <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rayLength));
        }

        _camera = camera ? camera : throw new ArgumentNullException(nameof(camera));
        _rayLength = rayLength;
    }

    public void CorrectTransformable(Transform fieldPosition, FieldSize fieldSize, FieldIntervals fieldIntervals)
    {
        float halfWidthOfScreen = Screen.width / 2f;

        Ray ray = _camera.ScreenPointToRay(new Vector3(halfWidthOfScreen, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, _rayLength))
        {
            fieldPosition.forward = -Vector3.ProjectOnPlane(_camera.transform.forward, hit.normal).normalized;

            fieldPosition.position = new Vector3(hit.point.x,
                                                 fieldPosition.position.y,
                                                 hit.point.z);

            fieldPosition.position += GetOffset(fieldPosition, fieldSize, fieldIntervals);
        }
    }

    private Vector3 GetOffset(Transform fieldPosition,
                              FieldSize fieldSize,
                              FieldIntervals fieldIntervals)
    {
        float halfOfField = fieldSize.AmountColumns / 2f;

        float offsetX = (halfOfField - MiddleOfModel) * fieldIntervals.BetweenColumns;

        float offsetZ = fieldIntervals.BetweenRows;

        return -fieldPosition.right * offsetX - fieldPosition.forward * offsetZ;
    }
}
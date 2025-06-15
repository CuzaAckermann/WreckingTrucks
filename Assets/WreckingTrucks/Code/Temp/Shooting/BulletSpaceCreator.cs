using UnityEngine;

public class BulletSpaceCreator : MonoBehaviour
{
    [Header("Bullet Mover Settings")]
    [SerializeField, Min(1)] private int _capacityMovables = 10;
    [SerializeField, Min(0.1f)] private float _movementSpeed = 35;
    [SerializeField, Min(0.001f)] private float _minSqrDistanceToTargetPosition = 0.001f;

    private Mover _bulletsMover;

    //public BulletSpace CreateBulletSpace(RoadSpace roadSpace)
    //{
    //    //Mover bulletsMover = new Mover();

    //    //BulletSpace bulletSpace = new BulletSpace();

    //    //return roadSpace;
    //}
}
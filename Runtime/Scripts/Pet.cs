using UnityEngine;

public class Pet : MonoBehaviour
{
    private readonly float _maxDistance = 0.2f;
    private readonly float _moveSpeed = 2f;

    private Transform _followTarget;

    private void OnEnable()
    {
        if (_followTarget != null)
        {
            transform.position = _followTarget.position;
        }
    }

    public void SetFollowPoint(Transform followPoint)
    {
        _followTarget = followPoint;
        transform.position = _followTarget.position;
    }

    private void FixedUpdate()
    {
        MovePet();
    }

    private void MovePet()
    {
        var targetPosition = Vector3.MoveTowards(transform.position, _followTarget.position, Time.fixedDeltaTime * _moveSpeed);
        var distance = Vector3.Distance(targetPosition, _followTarget.position);


        if (distance > _maxDistance)
        {
            targetPosition = _followTarget.position - (_followTarget.position - targetPosition).normalized * _maxDistance;
        }

        transform.position = targetPosition;

        var forward = _followTarget.position - transform.position;

        var direction = forward.sqrMagnitude > 0.04f ? forward.normalized : _followTarget.forward;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }
    }
}

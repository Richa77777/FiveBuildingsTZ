using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _buildingMask;
    [SerializeField] private MovePoint _moveArrow;

    private Camera _camera;
    private NavMeshAgent _agent;
    private Coroutine _moveCor;

    private void Awake()
    {
        _camera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SetMovePosition();
        }
    }

    private void SetMovePosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, _groundMask))
        {
            Vector3 targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);

            if (_moveCor != null) StopCoroutine(_moveCor);

            _moveArrow.EnableArrow(targetPos);
            _moveCor = StartCoroutine(MoveAndWait(targetPos));
        }

    }

    private IEnumerator MoveAndWait(Vector3 target)
    {
        _agent.SetDestination(target);

        while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
        {
            yield return null;
        }

        _moveArrow.DisableArrow();
    }
}

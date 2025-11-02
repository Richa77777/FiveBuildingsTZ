using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent), typeof(AudioSource))]
public class PlayerControl : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _buildingMask;
    [SerializeField] private MovePoint _moveArrow;

    [Header("Audio")]
    [SerializeField] private AudioClip _collectResourcesSound;

    [Header("Footsteps")]
    [SerializeField] private AudioClip[] _footstepSounds;
    [SerializeField] private float _footstepInterval = 0.5f;
    private float _footstepTimer;

    private Animator _animator;
    private NavMeshAgent _agent;
    private AudioSource _audioSource;

    private Camera _camera;
    private EventSystem _eventSystem;
    private Coroutine _moveCor;

    private bool _isCollecting = false;

    #region Unity Lifecycle

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();

        _camera = Camera.main;
        _eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (_isCollecting) return;

        HandleFootsteps();

        if (Input.GetMouseButtonUp(0) && !_eventSystem.IsPointerOverGameObject())
            HandleClick();
    }

    #endregion

    #region Click Handling

    private void HandleClick()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Click on building
        if (Physics.Raycast(ray, out hit, 1000f, _buildingMask))
        {
            if (hit.collider.TryGetComponent(out Building building))
                MoveToBuilding(building, hit.collider);
        }
        // Click on ground
        else if (Physics.Raycast(ray, out hit, 1000f, _groundMask))
        {
            MoveToPoint(hit.point);
        }
    }

    #endregion

    #region Movement

    private void MoveToPoint(Vector3 target)
    {
        if (_isCollecting) return;

        if (_moveCor != null)
            StopCoroutine(_moveCor);

        _moveArrow.EnableArrow(new Vector3(target.x, transform.position.y, target.z));
        _moveCor = StartCoroutine(MoveAndStop(target));
    }

    private void MoveToBuilding(Building building, Collider buildingCollider)
    {
        if (_isCollecting) return;

        if (_moveCor != null)
            StopCoroutine(_moveCor);

        Vector3 target = GetPointNearBuilding(buildingCollider, 1f);
        _moveArrow.EnableArrow(buildingCollider.bounds.center + Vector3.up * 2f);

        _moveCor = StartCoroutine(MoveAndInteract(building, target));
    }

    #endregion

    #region Coroutines

    private IEnumerator MoveAndStop(Vector3 target)
    {
        _animator.SetBool("IsWalking", true);
        _agent.SetDestination(target);

        while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
            yield return null;

        _moveArrow.DisableArrow();
        _animator.SetBool("IsWalking", false);
    }

    private IEnumerator MoveAndInteract(Building building, Vector3 target)
    {
        _animator.SetBool("IsWalking", true);
        _agent.SetDestination(target);

        while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
            yield return null;

        _moveArrow.DisableArrow();
        _animator.SetBool("IsWalking", false);

        int collected = building.CollectAllResources();

        if (collected > 0)
        {
            _isCollecting = true;

            StartCoroutine(RotateToBuildingSmooth(building));

            _audioSource.PlayOneShot(_collectResourcesSound);
            _animator.SetTrigger("Take");

            float takeAnimLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(takeAnimLength);

            _isCollecting = false;
        }
    }

    #endregion

    #region Utilities

    private IEnumerator RotateToBuildingSmooth(Building building, float rotationSpeed = 8f)
    {
        Vector3 directionToBuilding = (building.transform.position - transform.position).normalized;
        directionToBuilding.y = 0;

        if (directionToBuilding == Vector3.zero)
            yield break;

        Quaternion targetRotation = Quaternion.LookRotation(directionToBuilding);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private Vector3 GetPointNearBuilding(Collider buildingCollider, float distanceFromBuilding)
    {
        Vector3 buildingCenter = buildingCollider.bounds.center;
        Vector3 direction = (transform.position - buildingCenter).normalized;

        Vector3 target = buildingCenter + direction * (buildingCollider.bounds.extents.magnitude + distanceFromBuilding);
        target.y = transform.position.y;

        return target;
    }

    private void HandleFootsteps()
    {
        if (_animator.GetBool("IsWalking") && _agent.velocity.magnitude > 0.1f)
        {
            _footstepTimer += Time.deltaTime;

            if (_footstepTimer >= _footstepInterval)
            {
                PlayFootstepSound();
                _footstepTimer = 0f;
            }
        }
    }

    private void PlayFootstepSound()
    {
        if (_footstepSounds.Length == 0) return;

        AudioClip clip = _footstepSounds[Random.Range(0, _footstepSounds.Length)];
        _audioSource.pitch = Random.Range(0.95f, 1.05f);
        _audioSource.PlayOneShot(clip);
    }

    #endregion
}

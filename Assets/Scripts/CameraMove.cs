using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private void Update()
    {
        float moveDir = Input.GetAxisRaw("Horizontal");
        transform.position += Vector3.right * moveDir * _speed * Time.deltaTime;
    }
}
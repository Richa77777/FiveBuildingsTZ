using UnityEngine;

public class SkyboxParallax : MonoBehaviour
{
    [SerializeField] private float _parallaxFactor = 0.01f;

    private Material _skyboxMaterial;
    private Vector3 _lastCamPos;

    private void Start()
    {
        _skyboxMaterial = RenderSettings.skybox;
        _lastCamPos = transform.position;
    }

    private void Update()
    {
        ApplyParallax();
    }

    private void ApplyParallax()
    {
        Vector3 delta = transform.position - _lastCamPos;

        if (_skyboxMaterial.HasProperty("_Rotation"))
        {
            float rotation = _skyboxMaterial.GetFloat("_Rotation");
            rotation += delta.x * _parallaxFactor;
            _skyboxMaterial.SetFloat("_Rotation", rotation);
        }

        _lastCamPos = transform.position;
    }
}

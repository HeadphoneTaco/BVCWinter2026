using UnityEngine;

public class GhostVFX : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float lifetime = 2f;

    private SpriteRenderer _spriteRenderer;
    private Camera _mainCamera;
    private float _timer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _mainCamera = Camera.main;
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        // Float upward
        transform.position += Vector3.up * (floatSpeed * Time.deltaTime);

        // Billboard - face toward camera, not away from it
        transform.LookAt(
            transform.position + _mainCamera.transform.rotation * Vector3.forward,
            _mainCamera.transform.rotation * Vector3.up
        );

        // Fade out
        if (_spriteRenderer != null)
        {
            Color color = _spriteRenderer.color;
            color.a = Mathf.Lerp(1f, 0f, _timer / lifetime);
            _spriteRenderer.color = color;
        }

        if (_timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
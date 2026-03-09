using UnityEngine;

// Attach to a GameObject with a SpriteRenderer.
// Floats upward and fades out, then destroys itself.
// In Unity: create a GameObject, add SpriteRenderer, 
// assign your ghost PNG as a sprite, add this component.
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
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Always face the camera (billboard effect)
        transform.forward = _mainCamera.transform.forward;

        // Fade out
        Color color = _spriteRenderer.color;
        color.a = Mathf.Lerp(1f, 0f, _timer / lifetime);
        _spriteRenderer.color = color;

        if (_timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
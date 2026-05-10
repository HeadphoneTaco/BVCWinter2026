using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float cycleTime = 2f;
    [SerializeField] private Transform startpoint;
    [SerializeField] private Transform endpoint;
    
    private float _currentTime;
    private float _speed = 1f;
    
    private void Update()
    {
        _currentTime += _speed * Time.deltaTime;

        if (_currentTime < cycleTime) _speed = -1f;
        if (_currentTime > 0f) _speed = 1f;
        
        float t = Mathf.PingPong(Time.time / cycleTime, 1f);
        transform.position = Vector3.Lerp(startpoint.position, endpoint.position, t);
    }
}

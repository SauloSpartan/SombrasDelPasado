using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the camera tracking the player and other effects
public class CameraControl : MonoBehaviour
{
    // Variable for offset
    [SerializeField] private Vector3 _offset;
    private Vector3 _initialOffset;

    void Start()
    {
        _initialOffset = _offset;
    }

    void Update()
    {
        CameraFollow();
    }

    /// <summary>
    /// Function that makes the camera follow the player.
    /// </summary>
    private void CameraFollow()
    {
        transform.position = GameObject.Find("Character1").transform.position + _offset;
    }

    /// <summary>
    /// Coroutine that makes the camera shake, it receives two floats, one for duration and one for magnitude.
    /// </summary>
    /// <param name="duration"> Controls how long shake last.</param>
    /// <param name="magnitude"> Controls how strong shake is.</param>
    /// <returns></returns>
    public IEnumerator CameraShake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x_Position = Random.Range(-0.05f, 0.05f) * magnitude;
            float y_Position = Random.Range(-0.05f, 0.05f) * magnitude;

            _offset = new Vector3(_initialOffset.x + x_Position, _initialOffset.y + y_Position, _initialOffset.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        _offset = _initialOffset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    private Vector3 _spawnerPosition;
    private Vector3 _rightWall;
    private Vector3 _leftWall;

    private Transform _rightCollider;
    private Transform _leftCollider;

    void Awake()
    {
        _rightCollider = transform.Find("RightWall");
        _leftCollider = transform.Find("LeftWall");

        _rightCollider.transform.gameObject.SetActive(false);
        _leftCollider.transform.gameObject.SetActive(false);
    }

    void Update()
    {
        _spawnerPosition = transform.position;
        _rightWall = _spawnerPosition + _offset;
        _leftWall = _spawnerPosition - _offset;

        _rightCollider.transform.position = _rightWall;
        _leftCollider.transform.position = _leftWall;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _rightCollider.transform.gameObject.SetActive(true);
            _leftCollider.transform.gameObject.SetActive(true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_spawnerPosition, _rightWall);

        Color orange;
        ColorUtility.TryParseHtmlString("#ff8400", out orange);
        Gizmos.color = orange;
        Gizmos.DrawLine(_spawnerPosition, _leftWall);
    }
}

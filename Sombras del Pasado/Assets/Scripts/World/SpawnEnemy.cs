using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnEnemy : MonoBehaviour
{
    // Wall variables
    [Header("Wall Spacing")]
    [SerializeField] private Vector3 _offset;
    private Vector3 _spawnerPosition;
    private Vector3 _rightWall;
    private Vector3 _leftWall;
    private Transform _rightCollider;
    private Transform _leftCollider;

    [Header("Spawn Control")]
    [SerializeField] private float _spawnTimer; 
    [SerializeField] private GameObject _enemyBasic;
    [SerializeField] private int _basicQuantity;
    private Vector3 _enemySpawnRight;
    private Vector3 _enemySpawnLeft;

    void Awake()
    {
        _rightCollider = transform.Find("RightWall");
        _leftCollider = transform.Find("LeftWall");

        _rightCollider.transform.gameObject.SetActive(false);
        _leftCollider.transform.gameObject.SetActive(false);
    }

    void Update()
    {
        WallSpacing();
        SpawnSpacing();
    }

    private void WallSpacing()
    {
        _spawnerPosition = transform.position;
        _rightWall = _spawnerPosition + _offset;
        _leftWall = _spawnerPosition - _offset;

        _rightCollider.transform.position = _rightWall;
        _leftCollider.transform.position = _leftWall;
    }

    private void SpawnSpacing()
    {
        _enemySpawnRight = _spawnerPosition + (_offset * 3);
        _enemySpawnLeft = _spawnerPosition - (_offset * 3);
    }

    private IEnumerator SpawnControl()
    {
        int randomSpawner = 0;
        Vector3 randomPosition = _spawnerPosition;

        for (int i = 0; i < _basicQuantity; i++)
        {
            randomSpawner = Random.Range(0, 2);
            if (randomSpawner == 0)
            {
                randomPosition = _enemySpawnRight;
            }
            else if (randomSpawner == 1)
            {
                randomPosition = _enemySpawnLeft;
            }
            Instantiate(_enemyBasic, randomPosition, transform.rotation);
            yield return new WaitForSecondsRealtime(_spawnTimer);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _rightCollider.transform.gameObject.SetActive(true);
            _leftCollider.transform.gameObject.SetActive(true);
            StartCoroutine(SpawnControl());
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

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_enemySpawnRight, Vector3.one);

        Color green2;
        ColorUtility.TryParseHtmlString("#00ff7e", out green2);
        Gizmos.color = green2;
        Gizmos.DrawWireCube(_enemySpawnLeft, Vector3.one);
    }
}

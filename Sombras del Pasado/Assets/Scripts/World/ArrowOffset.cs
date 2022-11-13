using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowOffset : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    private Renderer _arrow;
    private WaitForSeconds _wait;

    private void Start()
    {
        _arrow = GetComponent<MeshRenderer>();
        _wait = new WaitForSeconds(0.7f);
    }

    void Update()
    {
        ArrowFollow();
    }

    /// <summary>
    /// Mantiene a la flecha con un offset de la camara
    /// </summary>
    private void ArrowFollow()
    {
        transform.position = GameObject.Find("Main Camera").transform.position + _offset;
    }

    public IEnumerator ArrowWink()
    {
        for (int t = 0; t < 6; t++)
        {
            _arrow.enabled = !_arrow.enabled;
            yield return _wait;
        }
    }
}

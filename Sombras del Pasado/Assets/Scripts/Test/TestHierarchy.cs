using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHierarchy : MonoBehaviour
{
    [SerializeField] private int _indexNumber;

    void Start()
    {
        _indexNumber = transform.GetSiblingIndex();
    }

    void Update()
    {
        transform.SetSiblingIndex(_indexNumber);
    }
}

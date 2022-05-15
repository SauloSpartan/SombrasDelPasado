using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKnockback : MonoBehaviour
{
    [SerializeField] float enemyThrust;
    [SerializeField] float knockTimer;
    [SerializeField] Rigidbody rigidbodyEnemy;
    [SerializeField] Transform playerPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Sword")
        {
            rigidbodyEnemy.isKinematic = false;
            Vector3 difference = rigidbodyEnemy.transform.position - playerPosition.transform.position;
            difference = difference.normalized * enemyThrust;
            rigidbodyEnemy.AddForce(difference, ForceMode.Impulse);
            StartCoroutine(KnockBack(rigidbodyEnemy));
        }
    }

    private IEnumerator KnockBack(Rigidbody rigidboyEnemy)
    {
        yield return new WaitForSeconds(knockTimer);
        rigidbodyEnemy.velocity = Vector3.zero;
        rigidbodyEnemy.isKinematic = true;
    }
}

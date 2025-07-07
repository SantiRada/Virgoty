using System.Collections;
using UnityEngine;

public class MeleeDamage : MonoBehaviour {

    public float startWait;
    public float endWait;

    private EnemyBase _enemy;

    private void Awake() { _enemy = GetComponent<EnemyBase>(); }
    private void Update()
    {
        if (_enemy.canAttack) { StartCoroutine(ThrowDamage()); }
    }
    private IEnumerator ThrowDamage()
    {
        yield return new WaitForSeconds(startWait);

        gameObject.tag = "Bullet";

        yield return new WaitForSeconds(endWait);

        gameObject.tag = "Enemy";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine("ThrowDamage()");
            gameObject.tag = "Enemy";
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine("ThrowDamage()");
            gameObject.tag = "Enemy";
        }
    }
}
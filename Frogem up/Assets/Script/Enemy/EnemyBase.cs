using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour {

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 3f;
    public float minDistanceToPlayer = 3f;

    [Header("Combat Settings")]
    public string nameOwner = "Enemy";
    public int damage = 5;
    public float attackCooldown = 2f;

    [Header("Debug")]
    public bool canMove = true;
    public bool canAttack = false;

    private PlayerMovement _player;
    private bool _isInAttackCooldown = false;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>();
    }
    private void Update()
    {
        if (_player == null) return;

        if (canMove)
        {
            MoveToPlayer();
        }

        RotateTowards();
    }
    private void MoveToPlayer()
    {
        Vector3 dirToPlayer = _player.transform.position - transform.position;
        dirToPlayer.y = 0f;

        float distanceToPlayer = dirToPlayer.magnitude;

        if (distanceToPlayer > minDistanceToPlayer)
        {
            Vector3 moveDirection = dirToPlayer.normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            canMove = true;
            canAttack = false;
        }
        else
        {
            canMove = false;
            canAttack = true;

            if (!_isInAttackCooldown) { StartCoroutine(AttackCooldownCoroutine()); }
        }
    }
    private IEnumerator AttackCooldownCoroutine()
    {
        _isInAttackCooldown = true;

        yield return new WaitForSeconds(attackCooldown);

        canMove = true;
        canAttack = false;
        _isInAttackCooldown = false;

        CheckPlayerDistance();
    }
    private void CheckPlayerDistance()
    {
        if (_player == null) return;

        Vector3 dirToPlayer = _player.transform.position - transform.position;
        dirToPlayer.y = 0f;
        float distanceToPlayer = dirToPlayer.magnitude;

        if (distanceToPlayer <= minDistanceToPlayer)
        {
            canMove = false;
            canAttack = true;

            if (!_isInAttackCooldown) { StartCoroutine(AttackCooldownCoroutine()); }
        }
    }
    private void RotateTowards()
    {
        Vector3 dirToTarget = (_player.transform.position - transform.position).normalized;
        dirToTarget.y = 0f;

        if (dirToTarget.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    public bool IsInAttackRange() { return canAttack && !canMove; }
    public bool IsMoving() { return canMove && !canAttack; }
    public float GetDistanceToPlayer()
    {
        if (_player == null) return float.MaxValue;

        Vector3 dirToPlayer = _player.transform.position - transform.position;
        dirToPlayer.y = 0f;
        return dirToPlayer.magnitude;
    }
}
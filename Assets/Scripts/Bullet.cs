using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private Rigidbody _rigidbody;
    private int _damage;
    private bool _hasHit = false;

    public void Init(Vector3 velocity, int damage = 0)
    {
        _damage = damage;
        _rigidbody.linearVelocity = velocity;
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Если уже попали в кого-то — игнорируем последующие столкновения
        if (_hasHit) return;

        int finalDamage = _damage;

        // Проверяем, если попали в голову
        if (collision.collider.CompareTag("Head"))
        {
            finalDamage = Mathf.RoundToInt(_damage * 2f);
        }

        // Проверяем, есть ли компонент EnemyCharacter на том, во что попали
        if (collision.collider.TryGetComponent(out EnemyCharacter enemy))
        {
            enemy.ApplyDamage(finalDamage);
            _hasHit = true;
        }
        // Или, если попали в дочерний объект (например, голову)
        else if (collision.collider.transform.root.TryGetComponent(out EnemyCharacter parentEnemy))
        {
            parentEnemy.ApplyDamage(finalDamage);
            _hasHit = true;
        }

        Destroy();
    }
}
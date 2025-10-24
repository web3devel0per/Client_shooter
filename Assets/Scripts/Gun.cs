using UnityEngine; // Assumed, since MonoBehaviour is used.
using System; // Assumed, since Action is used.

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected Bullet _bulletPrefab;
    public Action shoot;
}

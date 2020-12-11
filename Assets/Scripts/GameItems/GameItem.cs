using UnityEngine;

public abstract class GameItem : MonoBehaviour
{
    protected Collider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
    }

    public bool BlocksExplosion;
    public abstract void OnHit();
}

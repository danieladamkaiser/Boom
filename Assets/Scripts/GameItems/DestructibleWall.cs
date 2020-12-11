public class DestructibleWall : GameItem
{
    public override void OnHit()
    {
        Destroy(gameObject);
    }
}

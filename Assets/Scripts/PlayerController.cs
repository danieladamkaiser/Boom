using Assets.Scripts.Helpers;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : GameItem
{
    public GameObject bombPrefab;
    public float Speed;
    public Rigidbody Rb;
    public GameController GameController;
    public float Damage;
    public float Radius;
    public int MaxBombs;
    public float BombSpawnInterval;

    private float currentBombSpawnTimer;
    private int bombsCount;
    private Vector2 direction;
    private ParticleSystem ps;

    public void OnMove(InputAction.CallbackContext ctx) => direction = ctx.ReadValue<Vector2>();
    public void SpawnBomb(InputAction.CallbackContext ctx) => TryPlaceBomb();

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        bombsCount = MaxBombs;
        GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        transform.position = GameController.GetSpawnPosition();
    }

    private void TryPlaceBomb()
    {
        if (bombsCount > 0)
        {
            var roundedPosition = Vector3Int.RoundToInt(transform.position);
            var blockingItems = SpaceChecker.GetCollidingGameItems(roundedPosition, this.gameObject);
            if (blockingItems.Count() < 2)
            {
                var bomb = Instantiate(bombPrefab, roundedPosition, Quaternion.identity).GetComponent<Bomb>();
                bomb.SetUp(3, Radius, Damage);
                bombsCount -= 1;
            }
        }
    }

    private void Move(Vector2 direction)
    {
        Rb.velocity = Vector3.ClampMagnitude(new Vector3(direction.x, 0, direction.y), 1) * Speed;
    }

    private void Update()
    {
        RefillBombs();
        Move(direction);
    }

    private void RefillBombs()
    {
        if (bombsCount < MaxBombs)
        {
            currentBombSpawnTimer += Time.deltaTime;
            if (currentBombSpawnTimer > BombSpawnInterval)
            {
                currentBombSpawnTimer = 0;
                bombsCount++;
            }
        }
    }

    public override void OnHit()
    {
        Debug.Log($"Player {this} died");
    }
}

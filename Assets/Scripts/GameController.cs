using System;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject WallPrefab;
    public GameObject IndestructibleWallPrefab;
    public GameObject BombPrefab;
    public float DefaultTimer;
    public int Size;
    public ParticleSystem ParticleSystem;


    private int playersCount;
    private Vector3[] spawnPositions;
    private GameItem[,] gameItemsGrid;

    private void Awake()
    {
        SetSpawnPositions();
        gameItemsGrid = new GameItem[Size, Size];
        DontDestroyOnLoad(this.gameObject);
        CreateWorld();
    }

    private void SetSpawnPositions()
    {
        spawnPositions = new Vector3[] {
            new Vector3(-Size/2+1, 0, -Size/2+1),
            new Vector3(Size/2-1,0,-Size/2+1),
            new Vector3(-Size/2+1, 0,Size/2-1),
            new Vector3(Size/2-1,0,Size/2-1)
        };
    }

    public Vector3 GetSpawnPosition()
    {
        if (playersCount<spawnPositions.Count())
        return spawnPositions[playersCount++];

        return Vector3.zero;
    }

    public void CreateWorld()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (IsEdge(i, j) || (IsEven(i, j)) && IsNotRestricted(i, j))
                {
                    gameItemsGrid[i, j] = Instantiate(IndestructibleWallPrefab, new Vector3(i - Size / 2, 0, j - Size / 2), Quaternion.identity).GetComponent<GameItem>();
                }

                if (gameItemsGrid[i, j] == null && !IsCorner(i, j))
                {
                    gameItemsGrid[i, j] = Instantiate(WallPrefab, new Vector3(i - Size / 2, 0, j - Size / 2), Quaternion.identity).GetComponent<GameItem>();
                }
            }
        }
    }

    private bool IsCorner(int i, int j)
    {
        return (i < 3 && j < 3) || (i < 3 && j > Size - 4) || (i > Size - 4 && j < 3) || (i > Size - 4 && j > Size - 4);
    }

    private bool IsNotRestricted(int i, int j)
    {
        var middle = Size / 2;
        return (i < middle - 1 || i > middle + 1) && (j < middle - 1 || j > middle + 1);
    }

    private bool IsEven(int i, int j)
    {
        return i % 2 == 0 && j % 2 == 0;
    }

    private bool IsEdge(int i, int j)
    {
        return i == 0 || i == Size - 1 || j == 0 || j == Size - 1;
    }
}
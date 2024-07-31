using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

public class WorldGeography : MonoBehaviour
{
    private static readonly int WORLD_SIZE = 256;

    private static readonly int WATER_UP = 1 << 0;
    private static readonly int WATER_RIGHT = 1 << 1;
    private static readonly int WATER_DOWN = 1 << 2;
    private static readonly int WATER_LEFT = 1 << 3;
    private static readonly int TILE_ISLAND = WATER_UP | WATER_RIGHT | WATER_DOWN | WATER_LEFT;

    [Header("Perlin Noise Variables")]
    [SerializeField]
    [Range(1F, 10F)]
    private float scale;
    [SerializeField]
    [Range(0.25F, 0.75F)]
    private float threshold;

    [Header("World Generation")]
    [SerializeField]
    private GameObject landMap;
    [SerializeField]
    private GameObject waterMap;
    [SerializeField]
    private Tile waterSprite;
    [SerializeField]
    private Tile[] landSprites;

    [Header("Save Data")]
    [SerializeField]
    private GameObject skeletonPrefab;
    [SerializeField]
    private GameObject necromancerPrefab;

    private Tilemap landTilemap;
    private Tilemap waterTilemap;

    public Vector3 spawnLocation;

    void Start()
    {
        landTilemap = landMap.GetComponent<Tilemap>();
        waterTilemap = waterMap.GetComponent<Tilemap>();

        TileType[] worldMap = GenerateTiles();
        PaintWorld(worldMap);
        waterMap.AddComponent<TilemapCollider2D>();

        if (File.Exists(Application.persistentDataPath + "/data.json"))
        {
            using StreamReader reader = new(Application.persistentDataPath + "/data.json");
            LevelData data = LevelData.Load(reader.ReadToEnd());
            spawnLocation = data.PlayerLocation;
            

            foreach (Vector3 location in data.SkeletonLocations)
            {
                GameObject skeleton = Instantiate(skeletonPrefab);
                skeleton.transform.position = location;
            }

            foreach (Vector3 location in data.NecromancerLocations)
            {
                GameObject necromancer = Instantiate(necromancerPrefab);
                necromancer.transform.position = location;
            }
        }
        else
        {
            spawnLocation = GetSpawnLocation(worldMap);
        }
    }

    private TileType[] GenerateTiles()
    {
        TileType[] worldTiles = new TileType[WORLD_SIZE * WORLD_SIZE];

        for (int i = 0; i < worldTiles.Length; i++)
        {
            worldTiles[i] = Mathf.PerlinNoise(i % WORLD_SIZE / (float)WORLD_SIZE * scale, (float)i / WORLD_SIZE / WORLD_SIZE * scale) < threshold ? new TileType(false) : new TileType(true);

            if (i % WORLD_SIZE > 0)
            {
                if (worldTiles[i].IsWaterTile() && !worldTiles[i - 1].IsWaterTile())
                {
                    worldTiles[i - 1].SetWaterBound(WATER_RIGHT, true);
                }
                else if (!worldTiles[i].IsWaterTile() && worldTiles[i - 1].IsWaterTile())
                {
                    worldTiles[i].SetWaterBound(WATER_LEFT, true);
                }
            }
            if (i / WORLD_SIZE > 0)
            {
                if (worldTiles[i].IsWaterTile() && !worldTiles[i - WORLD_SIZE].IsWaterTile())
                {
                    worldTiles[i - WORLD_SIZE].SetWaterBound(WATER_UP, true);
                }
                else if (!worldTiles[i].IsWaterTile() && worldTiles[i - WORLD_SIZE].IsWaterTile())
                {
                    worldTiles[i].SetWaterBound(WATER_DOWN, true);
                }
            }
        }

        return worldTiles;
    }

    private void PaintWorld(TileType[] tiles)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].IsWaterTile())
            {
                waterTilemap.SetTile(new Vector3Int(i % WORLD_SIZE, i / WORLD_SIZE, 0), waterSprite);
                continue;
            }

            if (landSprites[tiles[i].GetWaterBounds()] == null)
            {
                landTilemap.SetTile(new Vector3Int(i % WORLD_SIZE, i / WORLD_SIZE, 0), landSprites[0]);
            }
            else
            {
                landTilemap.SetTile(new Vector3Int(i % WORLD_SIZE, i / WORLD_SIZE, 0), landSprites[tiles[i].GetWaterBounds()]);
            }
        }
    }

    private Vector3 GetSpawnLocation(TileType[] worldMap)
    {
        bool foundSpawn = false;
        int i = (WORLD_SIZE * WORLD_SIZE) / 2;
        Vector3Int result = new Vector3Int(0, 0, 0);
        while (!foundSpawn)
        {
            if (!worldMap[i].IsWaterTile())
            {
                result = new Vector3Int(i / WORLD_SIZE, i / WORLD_SIZE, -1);
                foundSpawn = true;
            }
            else
            {
                i++;
            }
        }

        return result;
    }

    public class TileType
    {
        private int waterBounds = 0;

        private bool isWater;

        public TileType(bool water)
        {
            isWater = water;
        }

        public bool IsWaterTile()
        {
            return isWater;
        }

        public int GetWaterBounds()
        {
            return waterBounds;
        }

        public void SetWaterBound(int waterBoundary, bool isWater)
        {
            if (isWater)
            {
                waterBounds |= waterBoundary;
            }
            else
            {
                waterBounds &= -1 ^ waterBoundary;
            }
        }
    }
}

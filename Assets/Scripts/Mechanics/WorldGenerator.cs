using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using ZombieTiles.Enums;

namespace ZombieTiles.Mechanics
{
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 300)]
        private uint worldSize;
        [SerializeField]
        [Range(1, 100)]
        private uint sandGenerationProbability;

        [SerializeField]
        private Tilemap tilemap;

        [SerializeField]
        private GameObject ground;
        [SerializeField]
        private GameObject sand;
        [SerializeField]
        private GameObject water;

        private string GrassName;

        private Tile groundTile;
        private Tile sandTile;
        private Tile waterTile;

        private void Start()
        {
            GrassName = ground.name;

            Assert.IsNotNull(GrassName);

            groundTile = ScriptableObject.CreateInstance<Tile>();
            groundTile.gameObject = ground;

            sandTile = ScriptableObject.CreateInstance<Tile>();
            sandTile.gameObject = sand;

            waterTile = ScriptableObject.CreateInstance<Tile>();
            waterTile.gameObject = water;

            GenerateGround(worldSize);

            foreach (var riverDirection in (RiverDirection[])System.Enum.GetValues(typeof(RiverDirection)))
            {
                GenerateRiverAndSand(worldSize, 2 * worldSize / 3, worldSize / 2, worldSize / 2, riverDirection);
            }
        }

        private void GenerateGround(uint size)
        {
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), groundTile);
                }
            }
        }

        private void GenerateRiverAndSand(uint worldSize,
            uint maxRiverLength,
            uint x = 0,
            uint y = 0,
            RiverDirection riverDirection = RiverDirection.North)
        {
            Assert.IsTrue(x < worldSize);
            Assert.IsTrue(y < worldSize);

            var riverPointCoordinator = new RiverPointCoordinator(x, y, worldSize);

            for (var riverTileCounter = 0;
                 riverPointCoordinator.IsInsideOfWorld() && riverTileCounter < maxRiverLength;
                riverTileCounter++)
            {
                var tileCoordinates = new Vector3Int((int)riverPointCoordinator.X, (int)riverPointCoordinator.Y, 0);
                tilemap.SetTile(tileCoordinates, waterTile);
                SetSandTileIfNeeded(tileCoordinates);

                riverPointCoordinator.Move(riverDirection);
            }
        }

        private void SetSandTileIfNeeded(Vector3Int tileCoordinates)
        {
            foreach (var x in new[] { -1, 0, 1 })
            {
                foreach (var y in new[] { -1, 0, 1 })
                {
                    if ((x == 0 && y == 0) || Random.Range(0, 101) >= sandGenerationProbability)
                    {
                        continue;
                    }

                    var destinationTileCoordinates = tileCoordinates + new Vector3Int(x, y, 0);

                    if (0 <= destinationTileCoordinates.x && destinationTileCoordinates.x < worldSize &&
                        0 <= destinationTileCoordinates.y && destinationTileCoordinates.y < worldSize)
                    // ToDo: Rewrite IsInsideOfTheWorld
                    {
                        var destinationTile = tilemap.GetTile(destinationTileCoordinates) as Tile;

                        if (destinationTile.gameObject.name.Contains(GrassName))
                        {
                            tilemap.SetTile(destinationTileCoordinates, sandTile);
                        }
                    }
                }
            }
        }
    }
}
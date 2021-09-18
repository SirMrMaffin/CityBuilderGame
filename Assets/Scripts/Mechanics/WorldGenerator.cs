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
        private Tilemap tilemap;

        [SerializeField]
        private GameObject ground;
        [SerializeField]
        private GameObject water;

        private void Start()
        {
            GenerateGround(worldSize);

            foreach (var riverDirection in (RiverDirection[])System.Enum.GetValues(typeof(RiverDirection)))
            {
                GenerateRiver(worldSize, 2 * worldSize / 3, worldSize / 2, worldSize / 2, riverDirection);
            }
        }

        private void GenerateGround(uint size)
        {
            var groundTile = ScriptableObject.CreateInstance<Tile>();
            groundTile.gameObject = ground;

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), groundTile);
                }
            }
        }

        private void GenerateRiver(uint worldSize,
            uint maxRiverLength,
            uint x = 0,
            uint y = 0,
            RiverDirection riverDirection = RiverDirection.North)
        {
            Assert.IsTrue(x < worldSize);
            Assert.IsTrue(y < worldSize);

            var riverPointCoordinator = new RiverPointCoordinator(x, y, worldSize);

            var waterTile = ScriptableObject.CreateInstance<Tile>();
            waterTile.gameObject = water;

            for (var riverTileCounter = 0;
                 riverPointCoordinator.IsInsideOfWorld() && riverTileCounter < maxRiverLength;
                riverTileCounter++)
            {
                tilemap.SetTile(new Vector3Int((int) riverPointCoordinator.X, (int) riverPointCoordinator.Y, 0), waterTile);
                riverPointCoordinator.Move(riverDirection);
            }
        }
    }
}
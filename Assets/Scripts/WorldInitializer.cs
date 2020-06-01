using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes.Scripts
{
    public class WorldInitializer : MonoBehaviour
    {
        public WorldConfig config;
        public Queue<EncodedWorld> worldStates = new Queue<EncodedWorld>();

        public WorldMap GetInitialWorldState()
        {
            return worldStates.Any() ? CreateWorld(worldStates.Dequeue()) : GetRandomWorldState();
        }

        private WorldMap CreateWorld(EncodedWorld encoded)
        {
            var mapSize = (int) Math.Sqrt(encoded.code.Length);
            var map = new WorldMap(mapSize);

            for (var i = 0; i < encoded.code.Length; i++)
                map.SetCell(InstantiateCell(new Coords(i / mapSize, i % mapSize), encoded.DecodeState(i)));

            return map;
        }

        public WorldMap GetRandomWorldState()
        {
            var worldMap = new WorldMap(config.worldSize);
            for (var x = 0; x < config.worldSize; x++)
            for (var z = 0; z < config.worldSize; z++)
                if (x >= config.worldSize / 4 && x < 3 * config.worldSize / 4 && z >= config.worldSize / 4 &&
                    z < 3 * config.worldSize / 4)
                    worldMap.SetCell(InstantiateCell(new Coords(x, z), (Cell.State) Random.Range(0, 2)));
                else
                    worldMap.SetCell(InstantiateCell(new Coords(x, z), Cell.State.DEAD));
            return worldMap;
        }

        private Cell InstantiateCell(Coords coords, Cell.State state)
        {
            var cellObject = Instantiate(config.cellPrefab, new Vector3(coords.x, 0, coords.z), Quaternion.identity,
                transform);

            cellObject.gameObject.SetActive(Cell.State.ALIVE.Equals(state));

            return new Cell(coords, state, cellObject);
        }
    }
}
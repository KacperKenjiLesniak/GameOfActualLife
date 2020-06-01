using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Scripts
{
    public class StateVisualizer : MonoBehaviour
    {
        public WorldConfig config;
        private List<Transform> previousCells = new List<Transform>();
        public Coords rootCoords = new Coords(30, 30);

        public void Visualize(EncodedWorld encodedWorld)
        {
            DestroyPreviousVisualisation();

            var map = encodedWorld.Decode();

            foreach (var cell in map.GetCells())
            {
                var cellObject = Instantiate(config.cellPrefab,
                    new Vector3(cell.coords.x + rootCoords.x, 0, cell.coords.z + rootCoords.z), Quaternion.identity,
                    transform);

                cellObject.gameObject.SetActive(Cell.State.ALIVE.Equals(cell.state));

                previousCells.Add(cellObject);
            }
        }

        private void DestroyPreviousVisualisation()
        {
            foreach (var previousCell in previousCells) Destroy(previousCell.gameObject);
            previousCells.Clear();
        }
    }
}
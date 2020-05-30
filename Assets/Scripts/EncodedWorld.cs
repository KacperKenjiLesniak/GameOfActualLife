using System;
using System.Net;
using Unity.Collections;

namespace Scenes.Scripts
{
    public class EncodedWorld
    {
        public string code { get; }
        public EncodedWorld(WorldMap map)
        {
            code = EncodeMap(map);
        }

        private string EncodeMap(WorldMap map)
        {
            var builder = new System.Text.StringBuilder();
            foreach (var cell in map.GetCells())
            {
                builder.Append(EncodeState(cell.state));
            }
            return builder.ToString();
        }

        public WorldMap Decode()
        {
            var mapSize = (int) Math.Sqrt(code.Length);
            WorldMap map = new WorldMap(mapSize);

            for (var i = 0; i < code.Length; i++)
            {
                map.SetCell(new Cell(new Coords(i/mapSize, i%mapSize), DecodeState(code[i]),null));
            }

            return map;
        }

        private Cell.State DecodeState(char c)
        {
            switch (c)
            {
                case 'o':
                    return Cell.State.ALIVE;
                case ' ':
                    return Cell.State.DEAD;
                default:
                    return Cell.State.DEAD;
            }
        }

        private char EncodeState(Cell.State state)
        {
            switch (state)
            {
                case Cell.State.ALIVE:
                    return 'o';
                case Cell.State.DEAD:
                    return ' ';
                default:
                    return 'x';
            }
        }
    }
}
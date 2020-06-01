using System;
using System.Text;

namespace Scenes.Scripts
{
    public class EncodedWorld
    {
        public const char ALIVE = 'o';
        public const char DEAD = ' ';

        public EncodedWorld(string code)
        {
            this.code = code;
        }

        public EncodedWorld(WorldMap map)
        {
            code = EncodeMap(map);
        }

        public string code { get; }

        private string EncodeMap(WorldMap map)
        {
            var builder = new StringBuilder();
            foreach (var cell in map.GetCells()) builder.Append(EncodeState(cell.state));

            return builder.ToString();
        }

        public WorldMap Decode()
        {
            var mapSize = (int) Math.Sqrt(code.Length);
            var map = new WorldMap(mapSize);

            for (var i = 0; i < code.Length; i++)
                map.SetCell(new Cell(new Coords(i / mapSize, i % mapSize), DecodeState(code[i]), null));

            return map;
        }

        public Cell.State DecodeState(int i)
        {
            return DecodeState(code[i]);
        }

        private Cell.State DecodeState(char c)
        {
            switch (c)
            {
                case ALIVE:
                    return Cell.State.ALIVE;
                case DEAD:
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
                    return ALIVE;
                case Cell.State.DEAD:
                    return DEAD;
                default:
                    return 'x';
            }
        }
    }
}
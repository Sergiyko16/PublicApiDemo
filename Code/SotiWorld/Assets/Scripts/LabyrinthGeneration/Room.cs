using System.Linq;

namespace Assets.Scripts.LabyrinthGeneration
{
    public class Room
    {
        private Matrix _matrix;
        private string _name;
        private string _color;

        public Room(int width, string name, string color)
        {
            _name = name;
            _color = color;

            this._matrix = new Matrix(width, Settings.DefaultHeight + Settings.RoomCoridorLenght);

            int firstEntranceWall = this._matrix.Tiles.Count / 2 - 1* Settings.Scale;
            int secondEntranceWall = this._matrix.Tiles.Count / 2 + 1 * Settings.Scale;

            int doorCenter = firstEntranceWall + (secondEntranceWall - firstEntranceWall)/2;

            for (int i = 0; i < this._matrix.Tiles.Count; i++)
            {
                for (int j = 0; j < this._matrix.Tiles[i].Count; j++)
                {
                    if ((i == 0 && j > Settings.RoomCoridorLenght) || j == Settings.RoomCoridorLenght
                        || (i == _matrix.Tiles.Count - 1 && j > Settings.RoomCoridorLenght)
                        || j == _matrix.Tiles[i].Count - 1)
                    {
                        _matrix.Tiles[i][j].Add(new Tile(TileType.Wall));
                    }

                    if (j >= 0 && j <= Settings.RoomCoridorLenght && i > firstEntranceWall && i < secondEntranceWall)
                    {
                        _matrix.Tiles[i][j].Add(new Tile(TileType.Door));
                    }

                    if ((i == firstEntranceWall && j < Settings.RoomCoridorLenght) || (i == secondEntranceWall && j < Settings.RoomCoridorLenght))
                    {
                        _matrix.Tiles[i][j].Add(new Tile(TileType.Wall));
                    }

                    if (i == doorCenter && j == 0)
                    {
                        var tile = new Tile(TileType.Text);
                        tile.Text = _name;
                        tile.Color = _color;

                        _matrix.Tiles[i][j].Add(tile);
                    }

                    if (i == 0 && j == (_matrix.Tiles[i].Count - Settings.RoomCoridorLenght) / 2 + Settings.RoomCoridorLenght)
                    {
                        _matrix.Tiles[i][j].Add(new Tile(TileType.Iphone) {Text = "Device Name"});
                    }

                    bool hasWalls = _matrix.Tiles[i][j].Any(t => t.TileType == TileType.Wall);

                    if (!hasWalls)
                    {
                        _matrix.Tiles[i][j].Add(new Tile(TileType.Floor));
                    }
                }
            }
        }

        public Matrix AsMatrix()
        {
            return this._matrix;
        }
    }
}
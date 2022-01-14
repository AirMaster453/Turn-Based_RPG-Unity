using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PsychesBound
{
    public class BattleField : MonoBehaviour
    {
        const int playerLimit = 5;

        [SerializeField]
        Color selectedTileColor = Color.cyan;

        [SerializeField]
        Color defaultTileColor = Color.white;

        [SerializeField]
        private Point size = new Point(10, 10);

        [SerializeField]
        private List<Point> playerPos = new List<Point>();

        [SerializeField]

        private List<Point> enemyPos = new List<Point>();


        [SerializeField]
        private Tile tilePrefab;

        private List<Tile> tiles = new List<Tile>(100);

        private Dictionary<Point, Tile> tileDict = new Dictionary<Point, Tile>(100);

        private bool tilesSet = false;

        public bool TileSet()
        {
            return tilesSet;
        }

        public void PlacePlayers(params Unit[] units)
        {
            for(int i = 0; i < units.Length && i < playerPos.Count && i < playerLimit; i++)
            {
                var tile = GetTileAt(playerPos[i]);
                Debug.Log($"tile: {tile}");
                units[i].Place(tile, this);
                units[i].Match();
            }
        }

        public void PlaceEnemies(params Unit[] units)
        {
            for (int i = 0; i < units.Length && i < enemyPos.Count && i < playerLimit; i++)
            {
                units[i].Place(GetTileAt(enemyPos[i]), this);
                units[i].Match();
            }
        }

        public Tile GetTileAt(Point position)
        {
            Tile tile;
            tileDict.TryGetValue(position, out tile);
            return tile;
        }

        public void SelectTiles(List<Tile> tiles)
        {
            for (int i = tiles.Count - 1; i >= 0; --i)
                tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
        }
        public void DeSelectTiles(List<Tile> tiles)
        {
            for (int i = tiles.Count - 1; i >= 0; --i)
                tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
        }

        public List<Tile> Search(IBattler unit, Tile start, Func<Tile, Tile, IBattler, bool> addTile)
        {
            List<Tile> retValue = new List<Tile>();
            retValue.Add(start);
            // Add more code here

            ClearSearch();
            Queue<Tile> checkNext = new Queue<Tile>();
            Queue<Tile> checkNow = new Queue<Tile>();

            start.distance = 0;
            checkNow.Enqueue(start);

            Point[] dirs = new Point[4]
            {
                new Point(0, 1),
                new Point(0, -1),
                new Point(1, 0),
                new Point(-1, 0)
            };
   
            while (checkNow.Count > 0)
            {
                Tile t = checkNow.Dequeue();
                // Add more code here

                for (int i = 0; i < 4; ++i)
                {
                    Tile next = GetTileAt(t.position + dirs[i]);
                    // Add more code here

                    if (next == null || next.distance <= t.distance + 1)
                        continue;

                    if (addTile(t, next, unit))
                    {
                        next.distance = t.distance + 1;
                        next.prev = t;
                        checkNext.Enqueue(next);
                        retValue.Add(next);
                    }
                }

                if (checkNow.Count == 0)
                    SwapReference(ref checkNow, ref checkNext);
            }

            
            return retValue;
        }

        void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
        {
            Queue<Tile> temp = a;
            a = b;
            b = temp;
        }
        public IEnumerable<Point> GetAllPaths(Unit unit)
        {
            return null;
        }

        public void InitializeTiles()
        {
            for(int i = 0; i < tiles.Count; i++)
            {
                DestroyImmediate(tiles[i]);
                //tiles.RemoveAt(i);
            }

            tiles.Clear();

            for(int x = 1; x <= size.x; x++)
            {
                for(int y = 1; y <= size.y; y++)
                {
                    var tile = Instantiate(tilePrefab, transform);
                    tile.position = new Point(x, y);
                    tiles.Add(tile);
                    tile.transform.localPosition = new Vector3(x, 0, y);
                }
            }

            tileDict.Clear();

            foreach (var tile in tiles)
            {
                tileDict.Add(tile.position, tile);
            }
        }

        void ClearSearch()
        {
            foreach (Tile t in tileDict.Values)
            {
                t.prev = null;
                t.distance = int.MaxValue;
            }
        }


        private void Start()
        {
            tileDict.Clear();

            tiles = new List<Tile>(GetComponentsInChildren<Tile>());

            foreach(var tile in tiles)
            {
                tileDict.Add(tile.position, tile);
                //Debug.Log("Tile FILLED");
            }

            tilesSet = true;
        }


        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < playerPos.Count; i++)
            {
                //var tile = GetTileAt(playerPos[i]).transform;
                Gizmos.color = Color.blue;
                var pos = playerPos[i] + transform.localPosition;
                Gizmos.DrawCube(pos, Vector3.one);

                Gizmos.color = Color.green;
                for (int j = 1; j <= (i+1); j++)
                {

                    Gizmos.DrawSphere(new Vector3(pos.x /*- ((j - 1) / 20)*/, pos.y + 0.4f * j, pos.z), 0.05f);
                }
            }

            for (int i = 0; i < enemyPos.Count; i++)
            {
                //var tile = GetTileAt(enemyPos[i]).transform;
                Gizmos.color = Color.red;
                var pos = enemyPos[i] + transform.localPosition;
                Gizmos.DrawCube(pos, Vector3.one);

                Gizmos.color = Color.green;
                for (int j = 1; j <= (i + 1); j++)
                {

                    Gizmos.DrawSphere(new Vector3(pos.x /*- ((j - 1)/20)*/, pos.y + 0.4f * j, pos.z), 0.05f);
                }
            }
        }

    }


#if UNITY_EDITOR
    [CustomEditor(typeof(BattleField))]
    public class BattleFieldEditor : Editor
    {
        public override UnityEngine.UIElements.VisualElement CreateInspectorGUI()
        {
            return base.CreateInspectorGUI();

            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            bool pressed = GUILayout.Button("Reload Tiles");

            var field = target as BattleField;

            if (pressed)
            {
                field.InitializeTiles();
            }
        }
    }
#endif
}
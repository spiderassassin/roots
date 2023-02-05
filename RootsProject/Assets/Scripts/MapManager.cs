using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap tiles;
    public Sprite dirt;
    public Sprite largeRock;
    public Sprite smallRock;
    public Sprite water;
    public Sprite phosphorus;
    public Sprite potassium;
    public Sprite nitrogen;
    public string nextLevel = "-1";

    private Dictionary<Vector3Int, DynamicObject> dynamicObjectsMap;

    private void Start()
    {
        if (nextLevel == "-1") print("WARNING: there is no next level index assigned");
        dynamicObjectsMap = new Dictionary<Vector3Int, DynamicObject>();
    }

    public void RegisterDynamicObject(GameObject go,TileType t) {
        dynamicObjectsMap.Add(grid.WorldToCell(go.transform.position), new DynamicObject(go,t));
    }

    public enum TileType { Dirt, LargeRock, SmallRock, Root, Water, Potassium, Phosphorus, Nitrogen, Unknown };
    public TileType GetTileType(Vector3 pos)
    {
        pos.z = 0;
        Vector3Int intPos = tiles.WorldToCell(pos);
        if (dynamicObjectsMap.ContainsKey(intPos)) {
            return dynamicObjectsMap[intPos].type;
        }

        Tile t = tiles.GetTile<Tile>(intPos);
        if (t.sprite == dirt) {
            return TileType.Dirt;
        }
        if (t.sprite == potassium)
        {
            return TileType.Potassium;
        }
        if (t.sprite == largeRock) {
            return TileType.LargeRock;
        }
        if (t.sprite == smallRock) {
            return TileType.SmallRock;
        }
        if (t.sprite == water) {
            return TileType.Water;
        }
        if (t.sprite == phosphorus) {
            return TileType.Phosphorus;
        }
        if (t.sprite == nitrogen) {
            return TileType.Nitrogen;
        }

        print(" the tile type is unknown!!!");
        return TileType.Unknown;
    }

    public void LoadNextLevel() 
    {
        SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }

    public class DynamicObject
    {
        public GameObject instance;
        public TileType type;
        public DynamicObject(GameObject g, TileType t) {
            instance = g;
            type = t;
        }
    }
}

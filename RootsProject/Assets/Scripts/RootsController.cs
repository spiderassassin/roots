using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsController : MonoBehaviour
{
    public Direction startingDirection;
    public MapManager map;
    public SpriteRenderer rootRenderer;
    public Transform root;
    public Sprite r, l, u, d;
    public GameObject rr, ll, uu, dd;

    public enum Direction { Up,Right,Left,Down};
    private Direction lastDirection;
    private Direction direction;

    private void Start()
    {
        direction = startingDirection;
        rootRenderer.sprite = GetSprite(direction);
    }
    Sprite GetSprite(Direction dir) {
        switch (dir) {
            case Direction.Down:
                return d;
            case Direction.Up:
                return u;
            case Direction.Right:
                return r;
            case Direction.Left:
                return l;
        }
        return rootRenderer.sprite;
    }
    private void Update()
    {
        //Get the movement vector based on input.
        Vector3 movement = Vector3.zero;
        float angle = 0;
        if (Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            movement = (Vector3.up);
            direction = Direction.Up;

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement = (-Vector3.up);
            angle = 180;
            direction = Direction.Down;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement = (Vector3.right);
            angle = -90;
            direction = Direction.Right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement = (-Vector3.right);
            angle = 90;
            direction = Direction.Left;
        }
        
        //Depending on the tile do some logic.
        if (movement.sqrMagnitude > 0) 
        {
            Vector3 newPosition = root.position + movement;
            MapManager.TileType t = map.GetTileType(newPosition);

            switch (t)
            {
                case MapManager.TileType.Dirt:
                    //Spawning new root.
                    GameObject prefab = null;
                    switch (lastDirection)
                    {
                        case Direction.Down:
                            prefab = dd;
                            break;
                        case Direction.Up:
                            prefab = uu;
                            break;
                        case Direction.Right:
                            prefab = rr;
                            break;
                        case Direction.Left:
                            prefab = ll;
                            break;
                    }
                    GameObject o = Instantiate(prefab);
                    o.transform.position = root.position;
                    map.RegisterDynamicObject(o, MapManager.TileType.Root);

                    root.position = newPosition;
                    rootRenderer.sprite = GetSprite(direction);
                    break;
            }

            lastDirection = direction;
        }
    }
}

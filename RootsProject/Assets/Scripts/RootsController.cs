using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RootsController : MonoBehaviour
{
    public Direction startingDirection;
    public MapManager map;
    public SpriteRenderer rootRenderer;
    public Transform root;
    public Sprite r, l, u, d;
    public GameObject rr, ll, uu, dd;
    public TextMeshProUGUI movesLeftText;
    public int movesLeft = 10;

    public enum Direction { Up = 0,Right = -90,Left = 90,Down = 180};

    private Root primaryRoot;
    private List<Root> roots;

    private void Start()
    {
        roots = new List<Root>();
        Root startingRoot = new Root(root, rootRenderer,startingDirection);
        primaryRoot = startingRoot;
        startingRoot.renderer.sprite = GetSprite(startingDirection);
        UpdateMovesLeft(movesLeft);
        roots.Add(startingRoot);
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

        foreach (Root r in roots)
        {
            Direction direction = r.direction;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                movement = (Vector3.up);
                direction = Direction.Up;

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                movement = (-Vector3.up);
                direction = Direction.Down;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                movement = (Vector3.right);
                direction = Direction.Right;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                movement = (-Vector3.right);
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
                        ApplyMove(r,newPosition,direction);
                        break;
                    case MapManager.TileType.Water:
                        ApplyMove(r,newPosition,direction);
                        map.LoadNextLevel();
                        break;
                    case MapManager.TileType.Potassium:
                        ApplyMove(r, newPosition, direction);
                        SpawnNewRoot(direction+90);
                        break;
                }
            }
        }
    }

    private void SpawnNewRoot(Direction direction) 
    {
        Transform t = Instantiate(new GameObject()).transform;
        SpriteRenderer r = t.gameObject.AddComponent<SpriteRenderer>();

        Root root = new Root(t, r, direction);
        roots.Add(root);
    }
    private void ApplyMove(Root r, Vector3 newPosition, Direction direction) 
    {
        r.direction = direction;

        //Spawning new root.
        GameObject prefab = null;
        switch (r.lastDirection)
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
        rootRenderer.sprite = GetSprite(r.direction);
        UpdateMovesLeft(movesLeft - 1);

        r.lastDirection = r.direction;

        // If there are no moves left we currently just restart the level.
        if (movesLeft <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }
    private void UpdateMovesLeft(int remainingMoves) 
    {
        movesLeft = remainingMoves;
        movesLeftText.text = "Moves Left: " + remainingMoves;
    }

    public class Root
    {
        public Transform transform;
        public SpriteRenderer renderer;

        public Direction lastDirection;
        public Direction direction;

        public Root(Transform t, SpriteRenderer r, Direction d) {
            transform = t;
            renderer = r;
            direction = d;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private int cell_size = 1;
    [SerializeField] private MazeCell cell;
    [SerializeField] private GameObject holder;

    void Start()
    {
        MazeCell[][] grid = new MazeCell[15][];
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = new MazeCell[10];
        }

        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                grid[i][j] = Instantiate(cell, new Vector3(j * cell_size, 0.5f, i * cell_size), Quaternion.identity, transform);
            }
        }

        GenerateMaze(grid);

        bool ball_spawned = false;
        while (!ball_spawned)
        {
            int chosen = Random.Range(0, grid.Length);
            MazeCell chosen_cell = grid[chosen][Random.Range(0, grid[chosen].Length)];

            if (chosen_cell.WallRemoved())
            {
                GameManager.GetInstance().SpawnBall(chosen_cell.transform.position);
                ball_spawned = true;
            }
        }
    }

    private void GenerateMaze(MazeCell[][] grid)
    {
        List<Vector2> frontier = new List<Vector2>();
        List<Vector2> marked = new List<Vector2>();

        int chosen_y = Random.Range(0, grid.Length);
        int chosen_x = Random.Range(0, grid[chosen_y].Length);

        Mark(new Vector2(chosen_x, chosen_y));
        while (frontier.Count > 0)
        {
            // Randomly choose a cell
            int index = Random.Range(0, frontier.Count);
            Vector2 chosen = frontier[index];
            frontier.RemoveAt(index);

            // Randomly choose one of its adjacent cells
            List<Vector2> adjacents = GetNeighbors(chosen);

            if (adjacents.Count > 0)
            {
                Vector2 chosen2 = adjacents[Random.Range(0, adjacents.Count)];

                Direction d = GetDirection(chosen, chosen2);

                grid[(int)chosen.y][(int)chosen.x].RemoveWall(d);
                grid[(int)chosen2.y][(int)chosen2.x].RemoveWall(GetOpposite(d));

                Mark(chosen);
            }
        }

        void Mark(Vector2 position)
        {
            marked.Add(position);

            AddToFrontier(position + Vector2.left);
            AddToFrontier(position + Vector2.right);
            AddToFrontier(position + Vector2.up);
            AddToFrontier(position + Vector2.down);
        }

        void AddToFrontier(Vector2 position)
        {
            if (position.x >= 0 
                && position.y >= 0 
                && position.y < grid.Length 
                && position.x < grid[(int)position.y].Length
                && !marked.Contains(position))
            {
                frontier.Add(position);
            }
        }
        
        Direction GetDirection(Vector2 pos1, Vector2 pos2)
        {
            if (pos1.x < pos2.x) return Direction.East;
            if (pos1.y < pos2.y) return Direction.North;
            if (pos1.x > pos2.x) return Direction.West;
            return Direction.South;
        }
        Direction GetOpposite(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.South:
                    return Direction.North;
                case Direction.East:
                    return Direction.West;
                default:
                    return Direction.East;
            }
        }

        List<Vector2> GetNeighbors(Vector2 position)
        {
            List<Vector2> res = new List<Vector2>();

            if (position.x > 0)
                res.Add(position + Vector2.left);

            if (position.x + 1 < grid[(int)position.y].Length)
                res.Add(position + Vector2.right);

            if (position.y > 0)
                res.Add(position + Vector2.down);

            if (position.y + 1 < grid[(int)position.y].Length)
                res.Add(position + Vector2.up);

            return res;
        }
    }

}

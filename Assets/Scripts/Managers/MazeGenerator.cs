using System;
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
        MazeCell[,] grid = new MazeCell[15, 10];
        Build(grid);

        GenerateMaze(grid, new Vector2(0, 4));
    }

    private void GenerateMaze(MazeCell[,] grid, Vector2 starting_position)
    {

        Stack<Vector2> positions = new Stack<Vector2>();
        HashSet<Vector2> visited = new HashSet<Vector2>();

        positions.Push(starting_position);

        StartCoroutine(BuildMaze(0.1f));
        
        IEnumerator BuildMaze(float wait_time)
        {
            while (positions.Count > 0)
            {
                Vector2 node = positions.Pop();
                if (visited.Contains(node)) continue;

                visited.Add(node);

                List<Vector2> neighbors = GetNeighbors(node);
                neighbors = neighbors.OrderBy(x => Guid.NewGuid()).ToList();

                Vector2 random_node = neighbors[Random.Range(0, neighbors.Count)];

                if (Random.Range(0, 2) == 1)
                {
                    grid[(int)node.x, (int)node.y].RemoveWall(GetDirection(node, random_node));
                    grid[(int)random_node.x, (int)random_node.y].RemoveWall(GetOpposite(GetDirection(node, random_node)));
                }

                bool pushed = false;
                foreach (Vector2 _ in neighbors)
                {
                    if (!visited.Contains(_))
                    {
                        pushed = true;
                        positions.Push(_);
                    }
                }

                yield return new WaitForSeconds(wait_time);

                if (pushed)
                {
                    Vector2 chosen = positions.Peek();

                    MazeCell c = grid[(int)chosen.x, (int)chosen.y];

                    grid[(int)node.x, (int)node.y].RemoveWall(GetDirection(node, chosen));
                    c.RemoveWall(GetOpposite(GetDirection(node, chosen)));
                }
            }

        }
        List<Vector2> GetNeighbors(Vector2 position)
        {
            List<Vector2> res = new List<Vector2>();

            if (position.x > 0) res.Add(new Vector2(position.x - 1, position.y));
            if (position.x + 1 < grid.GetLength(0)) res.Add(new Vector2(position.x + 1, position.y));
            if (position.y > 0) res.Add(new Vector2(position.x, position.y - 1));
            if (position.y + 1 < grid.GetLength(1)) res.Add(new Vector2(position.x, position.y + 1));

            return res;
        }

        Direction GetDirection(Vector2 initial, Vector2 destination)
        {
            if (initial.x + 1 == destination.x) return Direction.North;
            if (initial.x - 1 == destination.x) return Direction.South;
            if (initial.y + 1 == destination.y) return Direction.East;
            return Direction.West;
        }

        Direction GetOpposite(Direction dir)
        {
            switch (dir)
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
    }

    private void Build (MazeCell[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = Instantiate(cell, new Vector3(j * cell_size, 0.5f, i * cell_size), Quaternion.identity, holder.transform);
            }
        }
    }
}
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
    [SerializeField] private GameObject controlled_soldier;

    [SerializeField] private Vector2 starting_point;

    void Start()
    {
        MazeCell[,] grid = new MazeCell[14, 10];
        Build(grid);

        List<Vector2> goal_position = new List<Vector2>();
        goal_position.Add(new Vector2(13, 4));
        goal_position.Add(new Vector2(13, 5));

        GenerateMaze(grid, starting_point, goal_position);

        Vector3 spawn_ball_position = starting_point;
        for (int i = 0; i < 10; i++)
        {
            int x = Random.Range(0, grid.GetLength(0));
            int y = Random.Range(0, grid.GetLength(1));

            Vector3 pos = grid[x, y].transform.position;
            if (pos != grid[(int)starting_point.x, (int)starting_point.y].transform.position && 
                !goal_position.Contains(pos))
            {
                spawn_ball_position = pos;
                break;
            }
        }

        GameManager.GetInstance().SpawnBall(spawn_ball_position);

         Instantiate(controlled_soldier, grid[(int)starting_point.x, (int)starting_point.y].transform.position + Vector3.up * controlled_soldier.GetComponentInChildren<MeshRenderer>().bounds.extents.y, Quaternion.identity, transform);

        transform.parent = FindObjectOfType<GameField>().transform;
    }

    private void GenerateMaze(MazeCell[,] grid, Vector2 starting_position, List<Vector2> goal_position)
    {

        //Stack<Vector2> positions = new Stack<Vector2>();
        HashSet<Vector2> visited = new HashSet<Vector2>();

        //positions.Push(starting_position);
        DepthFirstTraversal(starting_position);

        // Support local functions
        void DepthFirstTraversal(Vector2 node)
        {
            //Vector2 node = positions.Pop();
            if (visited.Contains(node)) return;
            visited.Add(node);

            if (goal_position.Contains(node))
            {
                grid[(int)node.x, (int)node.y].RemoveWall(Direction.North);
            }

            List<Vector2> neighbors = GetNeighbors(node);
            neighbors = neighbors.OrderBy(x => Guid.NewGuid()).ToList();

            foreach (Vector2 _ in neighbors)
            {
                if (!visited.Contains(_))
                {
                    MazeCell c = grid[(int)_.x, (int)_.y];

                    grid[(int)node.x, (int)node.y].RemoveWall(GetDirection(node, _));
                    c.RemoveWall(GetOpposite(GetDirection(node, _)));

                    DepthFirstTraversal(_);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject north_wall;
    [SerializeField] private GameObject east_wall;
    [SerializeField] private GameObject south_wall;
    [SerializeField] private GameObject west_wall;

    public void RemoveWall(Direction direction)
    {
        switch(direction)
        {
            case Direction.North:
                north_wall.SetActive(false);
                break;
            case Direction.East:
                east_wall.SetActive(false);
                break;
            case Direction.South:
                south_wall.SetActive(false);
                break;
            case Direction.West:
                west_wall.SetActive(false);
                break;
        }
    }

    public bool WallRemoved() => !(north_wall.activeInHierarchy && east_wall.activeInHierarchy && south_wall.activeInHierarchy && west_wall.activeInHierarchy);

    public void RemoveAllWalls()
    {
        north_wall.SetActive(false);
        east_wall.SetActive(false);
        west_wall.SetActive(false);
        south_wall.SetActive(false);
    }
}

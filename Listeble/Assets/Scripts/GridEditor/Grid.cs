using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Строим уровень по координатам
/// </summary>
public class Grid : MonoBehaviour
{
  [SerializeField, Tooltip("")]
  private GameObject gridCellPrefab;

  [SerializeField, Tooltip("")]
  private Vector2Int vector2Int;

  private GameObject[,] gameGrid;

  //==============================================

  private void Start()
  {
    //AddObject();
  }

  private List<Vector2Int> vector2Ints = new List<Vector2Int>()
  {
    new Vector2Int(0, 0),
    new Vector2Int(1, 0),
    new Vector2Int(5, 6),
    new Vector2Int(3, 0),
    new Vector2Int(7, 6),
    new Vector2Int(3, 1)
  };

  //==============================================

  private void AddObject()
  {
    if (gridCellPrefab == null) { return; }

    gameGrid = new GameObject[vector2Int.x, vector2Int.y];

    for (int y = 0; y < vector2Int.y; y++)
    {
      for (int x = 0; x < vector2Int.x; x++)
      {
        foreach (var item in vector2Ints)
        {
          if (item.x == x && item.y == y)
          {
            gameGrid[x, y] = Instantiate(gridCellPrefab, new Vector3(x, 0, y), Quaternion.identity);
            gameGrid[x, y].transform.parent = transform;
            gameGrid[x, y].gameObject.name = $"Grid ( x: {x} y: {y} )";
          }
        }
      }
    }
  }

  /*private void CreateGrid()
  {
    if (gridCellPrefab == null) { return; }

    gameGrid = new GameObject[vector2Int.x, vector2Int.y];

    for (int y = 0; y < vector2Int.y; y++)
    {
      for (int x = 0; x < vector2Int.x; x++)
      {
        gameGrid[x, y] = Instantiate(gridCellPrefab, new Vector3(x, 0, y), Quaternion.identity);
        gameGrid[x, y].transform.parent = transform;
        gameGrid[x, y].gameObject.name = $"Grid ( x: {x} y: {y} )";
      }
    }
  }*/

  //==============================================



  //==============================================



  //==============================================
}
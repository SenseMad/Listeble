using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListBlocks", menuName = "Data/ListBlocks")]
public class ListBlocks : ScriptableObject
{
  [SerializeField, Tooltip("Список блоков объектов земли")]
  private List<Ground> _listGroundBlocks = new List<Ground>();

  //==============================================

  /// <summary>
  /// Список блоков объектов земли
  /// </summary>
  public List<Ground> ListGroundBlocks { get => _listGroundBlocks; set => _listGroundBlocks = value; }

  //==============================================

  [System.Serializable]
  public class EditorBlockData
  {
    //public Texture Texture;
    public GridObject PrefabGridObjects;
  }

  //==============================================
}
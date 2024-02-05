using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridEditorUI : MonoBehaviour
{
  [Header("������� ����")]
  [SerializeField, Tooltip("������ ��������� ������ ����")]
  private Button _gridLevelUpButton;
  [SerializeField, Tooltip("����� ������ ����")]
  private TextMeshProUGUI _gridLevelText;
  [SerializeField, Tooltip("������ ��������� ������ ����")]
  private Button _gridLevelDownButton;
  [SerializeField, Tooltip("���������� ��� ���������")]
  private Toggle _displayAllSubLevelToogle;

  [Header("������ ����")]
  [SerializeField, Tooltip("������ ���� X")]
  private TMP_InputField _fieldSizeX;
  [SerializeField, Tooltip("������ ���� Y")]
  private TMP_InputField _fieldSizeY;
  [SerializeField, Tooltip("������ ���� Z")]
  private TMP_InputField _fieldSizeZ;

  [Space(10)]
  [SerializeField, Tooltip("������ ���������")]
  private Button _saveButton;
  [SerializeField, Tooltip("������ ��������� �������")]
  private Button _loadButton;

  //==============================================

  private GridEditor GridEditor { get; set; }

  //==============================================

  private void Awake()
  {
    GridEditor = FindObjectOfType<GridEditor>();
  }

  private void OnEnable()
  {
    UpdateFloorLevel(GridEditor.GridLevel);
    UpdateFieldSizeText(GridEditor.FieldSize);
    GridEditor.OnChangeGridLevel.AddListener(UpdateFloorLevel);
    GridEditor.OnChangeFieldSize.AddListener(UpdateFieldSizeText);

    _saveButton.onClick.AddListener(() => GridEditor.SaveLevel(TypeLocation.Summer));
    _loadButton.onClick.AddListener(() => GridEditor.LoadLevel(TypeLocation.Summer, 0));

    _gridLevelUpButton.onClick.AddListener(() => ChangeFloorLevel(true));
    _gridLevelDownButton.onClick.AddListener(() => ChangeFloorLevel(false));

    _fieldSizeX.onValueChanged.AddListener(parValue => ChangeFieldSize());
    _fieldSizeY.onValueChanged.AddListener(parValue => ChangeFieldSize());
    _fieldSizeZ.onValueChanged.AddListener(parValue => ChangeFieldSize());
  }

  private void OnDisable()
  {
    GridEditor.OnChangeGridLevel.RemoveListener(UpdateFloorLevel);
    GridEditor.OnChangeFieldSize.RemoveListener(UpdateFieldSizeText);

    _saveButton.onClick.RemoveListener(() => GridEditor.SaveLevel(TypeLocation.Summer));
    _loadButton.onClick.RemoveListener(() => GridEditor.LoadLevel(TypeLocation.Summer, 0));

    _gridLevelUpButton.onClick.RemoveListener(() => ChangeFloorLevel(true));
    _gridLevelDownButton.onClick.RemoveListener(() => ChangeFloorLevel(false));

    _fieldSizeX.onValueChanged.RemoveListener(parValue => ChangeFieldSize());
    _fieldSizeY.onValueChanged.RemoveListener(parValue => ChangeFieldSize());
    _fieldSizeZ.onValueChanged.RemoveListener(parValue => ChangeFieldSize());
  }

  //==============================================

  #region ������ ����
  /// <summary>
  /// �������� ������ ����
  /// </summary>
  private void ChangeFieldSize()
  {
    if (int.TryParse(_fieldSizeX.text, out int x) && int.TryParse(_fieldSizeY.text, out int y) && int.TryParse(_fieldSizeZ.text, out int z))
    {
      GridEditor.ChangeFieldSize(new Vector3Int(x, y, z));
    }
  }

  /// <summary>
  /// �������� ����� ������� ����
  /// </summary>
  private void UpdateFieldSizeText(Vector3Int fieldSize)
  {
    _fieldSizeX.text = $"{fieldSize.x}";
    _fieldSizeY.text = $"{fieldSize.y}";
    _fieldSizeZ.text = $"{fieldSize.z}";
  }
  #endregion

  #region ������� ����
  /// <summary>
  /// �������� ������� ����
  /// </summary>
  private void ChangeFloorLevel(bool parValue)
  {
    GridEditor.ChangeGridLevel(parValue);
  }

  /// <summary>
  /// ������� ����� ������ ����
  /// </summary>
  private void UpdateFloorLevel(int gridLevel)
  {
    _gridLevelText.text = $"{gridLevel}";
  }
  #endregion

  /// <summary>
  /// ������ ���������
  /// </summary>
  /*private void ButtonSave()
  {
    _saveButton.onClick.AddListener(() => GridEditor.SaveLevel());
  }*/

  //==============================================
}
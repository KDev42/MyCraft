using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLayourSize : MonoBehaviour
{
    [SerializeField] float baseCellSize = 74.44f;
    [SerializeField] Vector2 baseParentSize = new Vector2(710, 400);
    [SerializeField] RectTransform parentTransdorm;

    private GridLayoutGroup gridLayout;

    private void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
    }

    private void Update()
    {
        ResizeChild();
    }

    private void ResizeChild()
    {
        float cureentSizeX = parentTransdorm.rect.width;
        float cureentSizeY = parentTransdorm.rect.height;
        if (cureentSizeX <= baseParentSize.x || cureentSizeY <= baseParentSize.y)
        {
            float cellSizeX = baseCellSize / (baseParentSize.x / cureentSizeX);
            float cellSizeY = baseCellSize / (baseParentSize.y / cureentSizeY);
            float cellSize = cellSizeX < cellSizeY ? cellSizeX : cellSizeY;
            gridLayout.cellSize = new Vector2(cellSize, cellSize);
        }
    }
}

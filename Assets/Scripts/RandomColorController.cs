using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RandomColorController : MonoBehaviour
{
    public Image[] images;
    public Button[] buttons;
    public int differentIndex;
    public int maxWidth = 600;
    public event Action onAnswerClick;
    public event Action onWrongClick;
    private GridLayoutGroup gridLayoutGroup;
    private int currentCellCount;
    // Start is called before the first frame update
    void Awake()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        images = GetComponentsInChildren<Image>();
        buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            //lamba
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    public void Refresh(int column, float colorRange)
    {
        UpdateGridLayout(column);
        SetRandomColor(colorRange);
    }
    private void UpdateGridLayout(int column)
    {
        int eachCellWidth = maxWidth / column;
        gridLayoutGroup.cellSize = Vector2.one * eachCellWidth;
        currentCellCount = column * column;
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(false);
            if (i < currentCellCount)
            {
                images[i].gameObject.SetActive(true);
            }
        }
    }
    private void SetRandomColor(float colorRange)
    {
        float randomH = UnityEngine.Random.value;
        float randomS = UnityEngine.Random.Range(0.6f + colorRange, 1f - colorRange);
        Color randomColor = Color.HSVToRGB(randomH, randomS, 1);
        float differentS = randomS + colorRange * (UnityEngine.Random.value > 0.5f ? 1f : -1f);
        Color differentColor = Color.HSVToRGB(randomH, differentS, 1);
        differentIndex = UnityEngine.Random.Range(0, currentCellCount);
        Debug.Log("Answer is at index " + differentIndex + ", randomS " + randomS + ", differentS " + differentS);
        for (int i = 0; i < currentCellCount; i++)
        {
            if (i == differentIndex)
                images[i].color = differentColor;
            else
                images[i].color = randomColor;
        }
    }

    private void OnButtonClick(int index)
    {
        Debug.Log("Click " + index);
        if (index == differentIndex)
            onAnswerClick?.Invoke();
        else
            onWrongClick?.Invoke();
    }
}

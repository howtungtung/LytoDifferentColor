using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
public class RandomColorController : MonoBehaviour
{
    private Image[] images;
    private Button[] buttons;
    public GridLayoutGroup gridLayoutGroup;
    public int maxWidth = 600;
    public int differentIndex;
    private int cellCount;
    public event Action onAnswerClick;
    public event Action onWrongClick;

    // Start is called before the first frame update
    void Start()
    {
        images = GetComponentsInChildren<Image>();
        buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnClick(index));
            buttons[i].gameObject.SetActive(false);
        }
    }

    public void Refresh(int colume, float colorRange)
    {
        cellCount = colume * colume;
        UpdateLayout(colume);
        SetRandomColor(colorRange);
    }

    private void UpdateLayout(int column)
    {
        int eachCellWidth = maxWidth / column;
        gridLayoutGroup.cellSize = Vector2.one * eachCellWidth;
        DOTween.Kill(this);
        for (int i = 0; i < images.Length; i++)
        {
            if (i < cellCount)
            {
                images[i].gameObject.SetActive(true);
                images[i].transform.localScale = Vector3.zero;
                images[i].transform.DOScale(Vector3.one, 0.3f).SetId(this);
            }
            else
                images[i].gameObject.SetActive(false);
        }
    }

    private void SetRandomColor(float colorRange)
    {
        float randomH = UnityEngine.Random.value;
        float randomS = UnityEngine.Random.Range(0.6f + colorRange, 1f - colorRange);
        float differentS = randomS + colorRange * UnityEngine.Random.value > 0.5 ? 1 : -1;
        Color randomColor = Color.HSVToRGB(randomH, randomS, 1);
        Color differentColor = Color.HSVToRGB(randomH, differentS, 1);
        differentIndex = UnityEngine.Random.Range(0, cellCount);
        for (int i = 0; i < images.Length; i++)
        {
            if (i == differentIndex)
                images[i].color = differentColor;
            else
                images[i].color = randomColor;
        }
    }

    public void OnClick(int index)
    {
        if (index == differentIndex)
        {
            onAnswerClick?.Invoke();
        }
        else
        {
            onWrongClick?.Invoke();
        }
    }
}

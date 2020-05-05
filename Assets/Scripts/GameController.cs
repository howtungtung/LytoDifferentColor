using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class GameController : MonoBehaviour
{
    public RandomColorController randomColorController;
    public int beginTime = 20;
    public int addTime = 1;

    public GameObject startingUI;
    public GameObject endingUI;
    public Image wrongImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;
    public LevelSetting[] levelSettings;
    private int currentLevel;
    private int CurrentLevel
    {
        set
        {
            currentLevel = value;
            levelText.text = currentLevel.ToString();
        }
        get
        {
            return currentLevel;
        }
    }

    private float currentTime;
    private float CurrentTime
    {
        set
        {
            currentTime = value;
            currentTime = Mathf.Max(0, currentTime);
            timeText.text = Mathf.RoundToInt(currentTime).ToString();
        }
        get
        {
            return currentTime;
        }
    }

    private IEnumerator Start()
    {
        yield return Starting();
        yield return Playing();
        yield return Ending();
    }

    public IEnumerator Starting()
    {
        currentTime = beginTime;
        startingUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        startingUI.SetActive(false);
    }

    private IEnumerator Playing()
    {
        randomColorController.onAnswerClick += OnAnswerClick;
        randomColorController.onWrongClick += OnWrongClick;
        var levelSetting = GetLevelSetting();
        randomColorController.Refresh(levelSetting.column, levelSetting.randomRange);
        while (CurrentTime > 0)
        {
            yield return null;
            CurrentTime -= Time.deltaTime;
        }
        randomColorController.onAnswerClick -= OnAnswerClick;
        randomColorController.onWrongClick -= OnWrongClick;
    }

    private IEnumerator Ending()
    {
        endingUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }

    private void OnAnswerClick()
    {
        CurrentLevel++;
        CurrentTime += addTime;
        var levelSetting = GetLevelSetting();
        randomColorController.Refresh(levelSetting.column, levelSetting.randomRange);
    }

    private void OnWrongClick()
    {
        CurrentTime -= addTime;
        var color = wrongImage.color;
        color.a = 1;
        wrongImage.color = color;
        wrongImage.DOFade(0, 0.25f);
    }

    private LevelSetting GetLevelSetting()
    {
        int index = Mathf.Min(currentLevel, levelSettings.Length - 1);
        return levelSettings[index];
    }
}

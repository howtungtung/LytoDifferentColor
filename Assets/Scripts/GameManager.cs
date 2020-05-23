using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public RandomColorController randomColorController;
    public GameObject startingUI;
    public GameObject endingUI;
    public GameObject wrongUI;
    public float gameDuration = 20;
    private float currentTime;
    //Property 屬性
    private float CurrentTime
    {
        set
        {
            currentTime = Mathf.Max(0, value);
            timeText.text = Mathf.RoundToInt(currentTime).ToString();
        }
        get
        {
            return currentTime;
        }
    }

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
    public float addTime = 1f;
    public Text levelText;
    public Text timeText;
    public LevelSetting[] levelSettings;
    void Start()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return Starting();
        yield return Playing();
        yield return Ending();
    }

    private IEnumerator Starting()
    {
        Debug.Log("Starting");
        CurrentTime = gameDuration;
        CurrentLevel = 1;
        randomColorController.gameObject.SetActive(false);
        startingUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        startingUI.SetActive(false);
    }

    private IEnumerator Playing()
    {
        randomColorController.gameObject.SetActive(true);
        randomColorController.onAnswerClick += OnAnswerClick;
        randomColorController.onWrongClick += OnWrongClick;
        LevelSetting levelSetting = GetLevelSetting();
        randomColorController.Refresh(levelSetting.column, levelSetting.colorRange);
        Debug.Log("Playing");
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
        Debug.Log("Ending");
        endingUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }

    private void OnAnswerClick()
    {
        CurrentLevel++;
        CurrentTime += addTime;
        LevelSetting levelSetting = GetLevelSetting();
        randomColorController.Refresh(levelSetting.column, levelSetting.colorRange);
    }

    private void OnWrongClick()
    {
        currentTime -= addTime;
        Debug.Log("Wrong Answer!!!");
        wrongUI.SetActive(false);
        wrongUI.SetActive(true);
    }

    private LevelSetting GetLevelSetting()
    {
        int index = CurrentLevel - 1;
        index = Mathf.Min(index, levelSettings.Length - 1);
        return levelSettings[index];
    }
}

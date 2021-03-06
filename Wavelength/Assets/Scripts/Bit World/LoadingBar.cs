﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    #region singleton
    static LoadingBar instance;
    static public LoadingBar Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        instance = this;
    }
    #endregion

    Text dialogue;
    Text percentage;
    RectTransform progressBar;
    Image progressBarImage;
    float progressBarGoal = 520.0f;

    Image[] images;

    CreationStage currentStage = CreationStage.none;
    float currentProgress = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

        // Set text components
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text t in texts)
        {
            switch (t.name)
            {
                case "ProgressText":
                    percentage = t;
                    break;
                case "DialogueText":
                    dialogue = t;
                    break;
                default:
                    break;
            }
        }
        // Set rect transform
        RectTransform[] rects = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform r in rects)
        {
            if (r.name == "LoadingBar")
            {
                progressBar = r;
                progressBarImage = progressBar.GetComponent<Image>();
                break;
            }
        }
        // Add self to delegate
        BitWorldMaker bwm = FindObjectOfType<BitWorldMaker>();
        if (bwm != null)
        {
            bwm.UpdateLoadScreenCall += UpdateLoadScreen;
        }
        // Find images in children
        images = GetComponentsInChildren<Image>();
    }

    void UpdateLoadScreen(CreationStage stage, float progress)
    {
        progress = Mathf.Clamp(progress, 0.0f, 1.0f);
        // If onto a new stage change text
        if (stage != currentStage)
        {
            BitWorldKnowledge knowledge = BitWorldKnowledge.Instance;
            switch (stage)
            {
                case CreationStage.destruction:
                    dialogue.color = knowledge.AirColourByWavelength[Wavelength.I];
                    progressBarImage.color = knowledge.AirColourByWavelength[Wavelength.VU];
                    dialogue.text = "Leaving previous area";
                    break;
                case CreationStage.creation:
                    dialogue.color = knowledge.AirColourByWavelength[Wavelength.V];
                    progressBarImage.color = knowledge.AirColourByWavelength[Wavelength.IU];
                    dialogue.text = "Finding new test";
                    break;
                case CreationStage.initialisation:
                    dialogue.color = knowledge.AirColourByWavelength[Wavelength.U];
                    progressBarImage.color = knowledge.AirColourByWavelength[Wavelength.IV];
                    dialogue.text = "Finalising details";
                    break;
                default:
                    dialogue.text = "See you space cowboy!";
                    break;
            }
        }
        // Update progress bar
        progressBar.sizeDelta = new Vector2(progressBarGoal * progress, progressBar.sizeDelta.y);
        //progressBar.sizeDelta = new Vector2((self.rect.width -  progressBarGoal) * (1.0f - progress), progressBar.sizeDelta.y);
        // Update % text
        percentage.text = ((int)(progress * 100)).ToString();

        // Save new values
        currentProgress = progress;
        currentStage = stage;
    }

    public void ShowLoadingScreen()
    {
        ShowHideVisuals(true);
        UpdateLoadScreen(CreationStage.destruction, 0.0f);
    }

    public void HideLoadingScreen()
    {
        ShowHideVisuals(false);
    }

    // Set false to hide, set true to show
    private void ShowHideVisuals(bool state)
    {
        // Images
        foreach (Image i in images)
        {
            i.enabled = state;
        }
        // Text
        percentage.enabled = state;
        dialogue.enabled = state;
    }
}

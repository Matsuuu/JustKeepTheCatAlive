using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkScript : MonoBehaviour
{
    public struct TalkObject
    {
        public string SpeakerName { get; set; }
        public Sprite SpeakerImage { get; set; }
        public string SpeakerContent { get; set; }
        public List<TalkDecision> Decisions { get; set; }

        public TalkObject(string speakerName, Sprite speakerImage, string speakerContent, List<TalkDecision> talkDecisions)
        {
            SpeakerName = speakerName;
            SpeakerImage = speakerImage;
            SpeakerContent = speakerContent;
            Decisions = talkDecisions;
        }
    }

    public struct TalkDecision
    {
        public string DecisionText { get; set; }

        public delegate void DecisionDelegate();

        public DecisionDelegate DecisionAction { get; set; }

        public TalkDecision(string decisionText, DecisionDelegate decisionAction)
        {
            DecisionText = decisionText;
            DecisionAction = decisionAction;
        }
    }
    
    public int textSpeed;

    public string textToWrite;

    public string textWritten;

    public Text textElement;

    public Image speakerImage;

    public GameObject choiceHud;
    public GameObject decisionObject;
    private Canvas canvas;
    public SpaceManPathFinder pathFinder;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        pathFinder = GameObject.FindGameObjectWithTag("SpaceMan").GetComponent<SpaceManPathFinder>();
    }

    public void LaunchDialog(TalkObject talkObject, bool clearDialog)
    {
        if (clearDialog)
        {
            textElement.text = "";
            textWritten = "";
        }
        else
        {
            textWritten = textElement.text;
        }
        SetSpeakerImage(talkObject.SpeakerImage);
        canvas.enabled = true;
        textToWrite = talkObject.SpeakerContent;
        pathFinder.DisableMovement();
        StartCoroutine(HandleDialog(0, talkObject));
    }

    private void SetSpeakerImage(Sprite newSpeakerImage)
    {
        if (newSpeakerImage == null)
        {
            Color transparent = Color.white;
            transparent.a = 0;
            speakerImage.color = transparent;
        }
        else
        {
            speakerImage.color = Color.white;
        }
        speakerImage.sprite = newSpeakerImage;
    }

    private IEnumerator HandleDialog(int talkStringIterator, TalkObject talkObject)
    {
        textWritten += textToWrite[talkStringIterator];
        textElement.text = textWritten;
        yield return new WaitForSeconds(60 / textSpeed);
        talkStringIterator++;
        if (talkStringIterator < textToWrite.Length)
        {
            yield return StartCoroutine(HandleDialog(talkStringIterator, talkObject));
            yield break;
        }

        if (talkObject.Decisions != null && talkObject.Decisions.Count > 0)
        {
            int offsetY = 10;
            foreach (TalkDecision talkObjectDecision in talkObject.Decisions)
            {
                GameObject choice = Instantiate(decisionObject, choiceHud.transform);
                choice.transform.localPosition = new Vector3(2,offsetY,0);
                offsetY -= 20;
                choice.GetComponent<Button>().onClick.AddListener(delegate { talkObjectDecision.DecisionAction(); });
                Text textElemOfChoice = choice.GetComponentInChildren<Text>();
                textElemOfChoice.text = talkObjectDecision.DecisionText;
            }
        }
    }

    public void CloseCanvas()
    {
        canvas.enabled = false;
        pathFinder.EnableMovement();
    }
}

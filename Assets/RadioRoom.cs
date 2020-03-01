using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RadioRoom : MonoBehaviour
{
    public TalkScript talkScript;
    // Start is called before the first frame update
    void Start()
    {
        talkScript = GameObject.FindGameObjectWithTag("TalkScriptHolder").GetComponent<TalkScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleRoomEnter()
    {
        talkScript.LaunchDialog(new TalkScript.TalkObject("Radio Room", null,
            "zzZZZzz..... ...ZZzzzzz... En taro adun",
            new List<TalkScript.TalkDecision> {
                new TalkScript.TalkDecision("Change station", ChangeStation),
                new TalkScript.TalkDecision("Leave", Leave)}), true);
    }

    private void ChangeStation()
    {
        Debug.Log("Change station");
        talkScript.CloseCanvas();
    }

    private void Leave()
    {
        Debug.Log("Leave");
        talkScript.CloseCanvas();
    }
}

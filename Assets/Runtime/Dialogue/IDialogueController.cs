using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueController
{
    void Pause();
    void Resume();
    void Start(string id);
    void Stop();
    bool ShowingDialog { get; }
    Action<string> DialogueComplete { get; set; }
}

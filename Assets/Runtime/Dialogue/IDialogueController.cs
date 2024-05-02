using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IDialogueController
{
    void Pause();
    void Resume();
    Task Start(string id);
    void Stop();
    bool ShowingDialog { get; }
    Action<string> DialogueComplete { get; set; }
}

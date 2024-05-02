using GameScript;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ActionLoadScene", menuName = "ScriptableObjects/ActionLoadScene", order = 1)]
public class ActionLoadScene : AAction
{
    public string Id;
}
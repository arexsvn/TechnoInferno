using GameScript;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ActionConversation", menuName = "ScriptableObjects/ActionConversation", order = 1)]
public class ActionConversation : AAction
{
    public ConversationReference Conversation;
}
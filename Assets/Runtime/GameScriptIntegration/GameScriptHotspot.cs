using GameScript;
using UnityEngine;

namespace GameScriptIntegration
{
    public class GameScriptHotspot : Hotspot
    {
        [SerializeField] private ConversationReference _conversationReference;
        public override string GetTextId()
        {
            return _conversationReference.Id.ToString();
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

public class Hotspot : MonoBehaviour
{
    public enum Type { Text, Move, Action/*,Item*/  };
    public Type type;
    public Scene destination;
    public Item item;
    [SerializeField] private string _textId;
    public HotspotView view;
    public List<string> hideForOutcomes;
    public List<string> showForOutcomes;

    public virtual string GetTextId()
    {
        return _textId;
    }
}

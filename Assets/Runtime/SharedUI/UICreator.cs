using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class UICreator
{
	private const string BUTTON_PATH = "UI/Button";
	private const string TEXT_AREA_PATH = "UI/TextArea";
	private const string DIALOG_PATH = "UI/DialogBox";
    private const string FADE_SCREEN_PREFAB = "UI/ScreenFade";
    private const string TEXT_OVERLAY_PREFAB = "UI/TextOverlay";
    private const string LIST_VIEW_PATH = "UI/SimpleListView";

    public ButtonView addButton(GameObject container, ButtonData buttonData)
    {
        string buttonPrefabResourceKey = BUTTON_PATH;

        if (buttonData.prefabResourceKey != null)
        {
            buttonPrefabResourceKey = buttonData.prefabResourceKey;
        }

        GameObject buttonPrefab = (GameObject)Object.Instantiate(Resources.Load(buttonPrefabResourceKey), container.transform);
        ButtonView buttonView = buttonPrefab.GetComponent<ButtonView>();
        string textColor = null;
        buttonView.button.onClick.RemoveAllListeners();
        buttonView.button.onClick.AddListener(buttonData.action);
        buttonView.labelText.text = buttonData.label;

        if (buttonData.textColor != null)
        {
            textColor = buttonData.textColor;
        }

        Color newColor;

        if (textColor != null)
        {
            ColorUtility.TryParseHtmlString(textColor, out newColor);
            buttonView.labelText.color = newColor;
        }

        if (buttonData.backgroundColor != null)
        {
            ColorUtility.TryParseHtmlString(buttonData.backgroundColor, out newColor);
            buttonView.buttonImage.color = newColor;
        }

        buttonView.button.interactable = buttonData.interactable;

        return buttonView;
    }

	public GameObject loadTextAreaPrefab(Transform parent = null)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load(TEXT_AREA_PATH), parent);

		return gameObject;
	}

    public GameObject createTextOverlay()
    {
        GameObject prefab = (GameObject)Object.Instantiate(Resources.Load(TEXT_OVERLAY_PREFAB));

        return prefab;
    }

    public GameObject createFadeScreen()
    {
        GameObject prefab = (GameObject)Object.Instantiate(Resources.Load(FADE_SCREEN_PREFAB));

        return prefab;
    }
}

/*
public class ButtonData
{
    public string label;
    public UnityAction action;

    public ButtonData(string label, UnityAction action)
    {
        this.label = label;
        this.action = action;
    }
}
*/
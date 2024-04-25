using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveGameSelectionView : MonoBehaviour
{
    public SaveGameEntryView savedGameEntry;
    public TextMeshProUGUI title;
    public Transform savedGameEntryContainer;
    public CanvasGroup canvasGroup;
    public Button closeButton;

    public SaveGameEntryView addSaveGame(SaveState saveGame)
    {
        SaveGameEntryView saveGameEntryView = Object.Instantiate(savedGameEntry, savedGameEntryContainer.transform);

        return saveGameEntryView;
    }

    public async void show(bool show = true, float fadeTime = -1)
    {
        float alpha = 1f;

        if (!show)
        {
            alpha = 0f;
        }

        await DOTweenUITransitions.fade(canvasGroup, alpha, fadeTime, gameObject, !show);
    }

    public void clearSaveGames()
    {
        foreach (Transform child in savedGameEntryContainer.transform)
        {
            SaveGameEntryView saveGameEntryView = child.GetComponent<SaveGameEntryView>();
            saveGameEntryView.deleteButton.onClick.RemoveAllListeners();
            Object.Destroy(child.gameObject);
        }
    }
}

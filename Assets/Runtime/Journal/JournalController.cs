using System;
using System.Collections.Generic;
using UnityEngine;

public class JournalController
{
    public Action CloseButtonClicked;
    private JournalView _view;
    private bool _showing = false;
    readonly CharacterManager _characterManager;
    readonly LocaleManager _localeManager;
    readonly AddressablesAssetService _addressablesAssetService;

    public JournalController(CharacterManager characterManager, LocaleManager localeManager, AddressablesAssetService addressablesAssetService)
    {
        _characterManager = characterManager;
        _localeManager = localeManager;
        _addressablesAssetService = addressablesAssetService;
    }

    public async void show(bool show = true, bool animate = true)
    {
        _showing = show;

        if (_view == null)
        {
            _view = await _addressablesAssetService.InstantiateAsync<JournalView>();
            _view.closeButton.onClick.AddListener(()=> CloseButtonClicked?.Invoke());
            _view.noEntriesOverlay.gameObject.SetActive(true);
        }

        if (show)
        {
            updateCharacters();
            _view.Show(animate);
        }
        else
        {
            _view.Hide(animate);
        }
        
    }

    private void updateCharacters()
    {
        _view.clearCharacters();
        _view.clearMemories();
        _view.clearMemoryOutcomes();
        _view.clearCharacterDetails();

        List<string> characterIds = _characterManager.GetCharacterIds();

        if (characterIds.Count > 0)
        {
            _view.noEntriesOverlay.gameObject.SetActive(false);

            foreach (string characterId in characterIds)
            {
                addCharacter(characterId);
            }

            openPage(characterIds[0]);
        }
        else
        {
            _view.noEntriesOverlay.gameObject.SetActive(true);
        }
    }

    private void addCharacter(string characterId)
    {
        PortraitView portrait = GameObject.Instantiate(_view.characterPortraitEntry, _view.characterListEntries.transform);
        string emotion = null;
        float totalStatus = _characterManager.GetTotalStatus(characterId);
        if (totalStatus > 0)
        {
            emotion = "happy";
        }
        else if(totalStatus < 0)
        {
            emotion = "angry";
        }

        portrait.display(characterId, emotion);
        portrait.button.onClick.AddListener(() => openPage(characterId));
    }

    private void openPage(string characterId)
    {
        _view.selectCharacter(characterId);
        showCharacterDetails(characterId);
        //showMemories(characterId);
    }

    private void showCharacterDetails(string characterId)
    {
        _view.clearCharacterDetails();

        CharacterBio characterBio = _characterManager.GetBio(characterId);

        _view.characterNameText.text = _localeManager.lookup(characterBio.characterName);

        foreach (CharacterDetail characterDetail in characterBio.details)
        {
            CharacterDetailView characterDetailView = GameObject.Instantiate(_view.characterDetailEntry, _view.characterDetailsEntries.transform);
            characterDetailView.detailText.text = _localeManager.lookup(characterDetail.detailLabel) + " : " + _localeManager.lookup(characterDetail.detailText);
        }
    }

    /*
    private void showMemories(string characterId)
    {
        _view.clearMemories();
       
        List<string> memoryIds = _memoryController.getMemoryIds(characterId);

        foreach (string memoryId in memoryIds)
        {
            Memory memory = _memoryController.getMemory(memoryId);
            MemoryEntryView memoryEntry = Object.Instantiate(_view.memoryEntry, _view.memoryListEntries.transform);
            memoryEntry.memoryTitle.text = _localeManager.lookup(memoryId + "_title");
            memoryEntry.button.onClick.AddListener(() => showOutcomes(memory));
        }

        showOutcomes(_memoryController.getMemory(memoryIds[0]));
    }

    private void showOutcomes(Memory memory)
    {
        _view.clearMemoryOutcomes();
        foreach (MemoryOutcome outcome in memory.outcomes)
        {
            _view.memoryOutcomesText.text += _localeManager.lookup(outcome.outcomeId) + "\n";
        }
    }
    */

    public bool showing
    {
        get
        {
            return _showing;
        }
    }
}

public class JournalCharacterPage
{
    public string characterId;
}

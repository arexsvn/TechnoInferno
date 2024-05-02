using GameScript;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameScriptIntegration
{
    public class DialogueController : IRunnerListener, IDialogueController
    {
        public Action<string> DialogueComplete { get; set; }
        private DialogueView _dialogueView;
        private ActiveConversation _activeConversation;
        readonly CoroutineRunner _coroutineRunner;
        readonly CharacterManager _characterManager;
        readonly SaveStateController _saveGameController;
        readonly AddressablesAssetService _assetService;
        readonly Runner _runner;
        readonly Locale _locale;
        readonly NodeMethods _nodeMethods;
        private static float TEXT_TIME_MULTIPLIER = 0.065f;
        private const string NARRATOR_NODE_NAME = "narrator";
        private const string PLAYER_NODE_NAME = "player";
        private const string CHARACTER_EMOTION_PROPERTY = "emotion";
        private static float MIN_TEXT_SECONDS = 2f;
        private bool _autoAdvanceDialogue = false;
        private bool _conversationActive = false;

        public DialogueController(CoroutineRunner coroutineRunner,
                                          CharacterManager characterManager,
                                          SaveStateController saveGameController,
                                          AddressablesAssetService assetService,
                                          Locale locale,
                                          Runner runner,
                                          NodeMethods nodeMethods)
        {
            _coroutineRunner = coroutineRunner;
            _characterManager = characterManager;
            _saveGameController = saveGameController;
            _assetService = assetService;
            _locale = locale;
            _runner = runner;
            _nodeMethods = nodeMethods;

            Init();
        }

        private async void Init()
        {
            _dialogueView = await _assetService.InstantiateAsync<DialogueView>();
            _dialogueView.gameObject.SetActive(false);
            _dialogueView.BackgroundClick += handleBackgroundClicked;
        }

        public async Task Start(string id)
        {
            _dialogueView.HideAll();
            _dialogueView.ClearChoices();
            _dialogueView.Show();

            _conversationActive = true;
            _activeConversation = Runner.StartConversation(uint.Parse(id), this);
            while(_conversationActive)
            {
                await Task.Delay(25);
            }
        }

        public void Pause()
        {

        }

        public void Resume()
        {

        }

        public void Stop()
        {
            Runner.StopConversation(_activeConversation);
        }

        public void OnConversationEnter(Conversation conversation, ReadyNotifier readyNotifier)
        {
            readyNotifier.OnReady();
        }

        public void OnConversationExit(Conversation conversation, ReadyNotifier readyNotifier)
        {
            _dialogueView.HideAll();
            readyNotifier.OnReady();
            _conversationActive = false;
            DialogueComplete?.Invoke(conversation.Id.ToString());
        }

        public void OnError(Conversation conversation, Exception e)
        {
            _conversationActive = false;
            Debug.LogError(e);
        }

        public async void OnNodeDecision(List<Node> nodes, DecisionNotifier decisionNotifier)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                Node node = nodes[i];
                if (node.UIResponseText == null)
                {
                    continue;
                }

                string choiceText = node.UIResponseText.GetLocalization(_locale);
                ChoiceView choiceView = await _assetService.InstantiateAsync<ChoiceView>(_dialogueView.ChoiceContainer.transform);
                choiceView.textArea.text = choiceText.Trim();
                choiceView.Clicked += () =>
                {
                    _dialogueView.ShowChoices(false);
                    _dialogueView.ClearChoices();
                    decisionNotifier.OnDecisionMade(node);
                };
            }

            _dialogueView.ShowChoices();
        }

        public void OnNodeEnter(Node node, ReadyNotifier readyNotifier)
        {
            if (node.VoiceText != null)
            {
                string actorName = node.Actor.LocalizedName != null ? node.Actor.LocalizedName.GetLocalization(_locale) : "<Player Name Missing>";
                string voiceText = node.VoiceText.GetLocalization(_locale);

                if (node.Actor.Name == NARRATOR_NODE_NAME)
                {
                    _dialogueView.SetDescriptiveText(voiceText);
                }
                else if (node.Actor.Name == PLAYER_NODE_NAME)
                {
                    bool isDecision = false;

                    if (node.OutgoingEdges != null && node.OutgoingEdges.Length > 0)
                    {
                        if (node.OutgoingEdges[0].Source.UIResponseText != null)
                        {
                            isDecision = true;
                        }
                    }

                    _dialogueView.SetPlayerText(voiceText, isDecision);
                }
                else
                {
                    ShowCharacterPortrait(node.Actor.Name, GetNodeProperty<string>(CHARACTER_EMOTION_PROPERTY, node.Properties));
                    _dialogueView.SetNpcText(voiceText);
                }

                float displayTime = int.MaxValue;
                if (_autoAdvanceDialogue)
                {
                    displayTime = Mathf.Max(voiceText.Length * TEXT_TIME_MULTIPLIER, MIN_TEXT_SECONDS);
                }

                _coroutineRunner.DelayUpdateAction(readyNotifier.OnReady, displayTime);
            }
            else
            {
                readyNotifier.OnReady();
            }
        }

        public void ShowCharacterPortrait(string name, string emotion = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _dialogueView.SetPortrait(name, emotion);  
            }
            else
            {
                _dialogueView.HidePortrait();
            }
        }

        private T GetNodeProperty<T>(string propertyName, Property[] properties)
        {
            if (properties == null)
            {
                return default;
            }

            foreach (var property in properties)
            {
                if (property.Name == propertyName)
                {
                    if (typeof(T) == typeof(bool))
                    {
                        return (T)(object)(property as BooleanProperty).GetBoolean();
                    }
                    if (typeof(T) == typeof(string))
                    {
                        return (T)(object)(property as StringProperty).GetString();
                    }
                    if (typeof(T) == typeof(int))
                    {
                        return (T)(object)(property as IntegerProperty).GetInteger();
                    }
                    if (typeof(T) == typeof(decimal))
                    {
                        return (T)(object)(property as DecimalProperty).GetDecimal();
                    }
                }
            }
            return default;
        }


        public void OnNodeExit(Node node, ReadyNotifier readyNotifier)
        {
            //_dialogueView.hideAll();
            readyNotifier.OnReady();
        }

        public bool ShowingDialog
        {
            get
            {
                return _dialogueView.isActiveAndEnabled;
            }
        }

        private void handleBackgroundClicked()
        {
            if (_coroutineRunner.RunningDelayedUpdateAction)
            {
                _coroutineRunner.RunDelayedUpdateActionNow();
            }
            /*
            if (_conversationFinished)
            {
                _dialogueView.hideAll();
            }
            */
        }
    }
}
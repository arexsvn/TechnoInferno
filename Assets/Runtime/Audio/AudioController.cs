using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController
{
    readonly AudioSources _audioSources;
    readonly SaveStateController _saveStateController;
    private System.Random _rand;

    public AudioController(AudioSources audioSources, SaveStateController saveStateController)
    {
        _audioSources = audioSources;
        _saveStateController = saveStateController;
    }

    public void init()
    {
        _audioSources.init();
        _rand = new System.Random();
    }

    public void play(string fileName, string type, float volumeScale = 1f)
    {
        float volumeModifier = 1f;

        if (type == AudioType.Music)
        {
            volumeModifier = _saveStateController.CurrentSave.MusicVolume;
        }
        else if (type == AudioType.Sfx)
        {
            volumeModifier = _saveStateController.CurrentSave.SfxVolume;
        }

        _audioSources.play(fileName, type, volumeScale * volumeModifier);
    }

    public void fade(string type, float targetVolume)
    {
        float duration = 2.0f;
        _audioSources.fade(type, duration, targetVolume);
    }

    public void setVolume(string type, float volume)
    {
        _audioSources.setVolume(type, volume);
    }
}

public class AudioType
{
    public const string Music = "Music";
    public const string Sfx = "Sfx";
}

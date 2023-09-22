using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public static class AudioSettings 
{
    private static EventInstance SFXVolumeTestEvent;

    private static Bus Music;
    private static Bus SFX;
    //private Bus Master;
    //private float MasterVolume = 1f;

    public static void Initialize()
    {
        //Music = RuntimeManager.GetBus("bus:/Master/Music");
        SFX = RuntimeManager.GetBus("bus:/Sounds");
        //Master = RuntimeManager.GetBus("bus:/Master");
        SFXVolumeTestEvent = RuntimeManager.CreateInstance("event:/SoundEvents/MineEvents/DropOrb");
    }

    //public void MasterVolumeLevel(float newMasterVolume)
    //{
    //    Master.setVolume(MasterVolume);
    //    MasterVolume = newMasterVolume;
    //}

    public static void MusicVolumeLevel(float newMusicVolume)
    {
        Music.setVolume(newMusicVolume);
    }

    public static void SFXVolumeLevel(float newSFXVolume)
    {
        SFX.setVolume(newSFXVolume);

        PLAYBACK_STATE PbState;
        SFXVolumeTestEvent.getPlaybackState(out PbState);
        if (PbState != PLAYBACK_STATE.PLAYING)
        {
            SFXVolumeTestEvent.start();
        }
    }
}

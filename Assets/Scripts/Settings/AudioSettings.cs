using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;

public static class AudioSettings 
{
    private static EventInstance SFXVolumeTestEvent;

    private static Bus Music;
    private static Bus SFX;
    private static Bus Master;
    //private Bus Master;
    //private float MasterVolume = 1f;

    private  static bool audioResumed = false;

    public static void Initialize()
    {
        //Music = RuntimeManager.GetBus("bus:/Master/Music");

        //if (false)
        {
            var result = FMODUnity.RuntimeManager.CoreSystem.mixerSuspend();
            Debug.Log(result);
            result = FMODUnity.RuntimeManager.CoreSystem.mixerResume();
            Debug.Log(result);
            audioResumed = true;

            try
            {
                Master = RuntimeManager.GetBus("bus:/");
            }
            catch (Exception) { Debug.Log("mb"); }

            try
            {
                SFX = RuntimeManager.GetBus("bus:/Sounds");
            }
            catch (Exception) { Debug.Log("m b/s"); }
        }
        //Master = RuntimeManager.GetBus("bus:/Master");
        //SFXVolumeTestEvent = RuntimeManager.CreateInstance("event:/SoundEvents/MineEvents/DropOrb");
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
        //Debug.Log("audio = " + newSFXVolume);
        try
        {
            SFX.setVolume(newSFXVolume);
        }
        catch (Exception) { }

        //PLAYBACK_STATE PbState;
        //SFXVolumeTestEvent.getPlaybackState(out PbState);
        //if (PbState != PLAYBACK_STATE.PLAYING)
        //{
        //    SFXVolumeTestEvent.start();
        //}
    }
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour {

	public AudioMixer Mixer;
	public AudioMixerSnapshot[] Snapshots;

    public AudioSource JumpSfx;
    public AudioSource DeathSfx;

	private float timeToReach;

	void Awake()
	{
		if (Mixer == null) throw new MissingReferenceException ("AudioController is missing reference to the Mixer AudioMixer.");
		if (Snapshots == null) throw new MissingReferenceException ("AudioController is missing reference to the Snapshots AudioMixerSnapshots.");

        if (JumpSfx == null) throw new MissingReferenceException("AudioController is missing reference to the JumpSfx AudioSource.");
        if (DeathSfx == null) throw new MissingReferenceException("AudioController is missing reference to the DeathSfx AudioSource.");

		timeToReach = 2f;
	}
	
	private void Transition(float[] weights)
	{
		Mixer.TransitionToSnapshots(Snapshots, weights, timeToReach);
    }

    public void Menu()
    {
        Transition(new float[] { 1, 0, 0});
    }
    public void InGame()
    {
        Transition(new float[] { 0, 1, 0 });
    }
    public void GameOver()
    {
        Transition(new float[] {  0, 0, 1 });
        DeathSfx.Play();
    }

    private float Volume(bool isOn)
    {
        return isOn ? 0 : -80;
    }

	private float VolumeSf(bool isOn)
	{
		return isOn ? -20 : -80;
	}
    public void MusicToggle(bool isEnabled)
    {
        Mixer.SetFloat("musicVolume", Volume(isEnabled));
    }
    public void SfxToggle(bool isEnabled)
    {
        Mixer.SetFloat("sfxVolume", VolumeSf(isEnabled));
    }

    public void Jump()
    {
        JumpSfx.Play();
    }
}
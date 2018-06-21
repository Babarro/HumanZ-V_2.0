using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {

	public Sound[] sounds;
	// Use this for initialization
	void Awake () {
		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource> ();
			s.source.clip = s.clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			 
		}
	}

	void Start(){
		PlaySound("InGameTheme");
	}
	
	public void PlaySound(string name){
		Sound s = Array.Find (sounds, sound => sound.name == name);
		s.source.Play ();
	}

	public void StopSound(string name){
		Sound s = Array.Find (sounds, sound => sound.name == name);
		s.source.Stop ();
	}
}

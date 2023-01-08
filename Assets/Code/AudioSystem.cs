using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
	private static AudioSystem _instance;
	public static AudioSystem Instance { get { return _instance; } }

	public AudioSource SfxSource;
	public AudioSource MusicSource;

	[Header("Music"), SerializeField] private AudioClip level1Theme;
	[SerializeField] private AudioClip level2Theme;
	[SerializeField] private AudioClip level3Theme;
	[SerializeField] private AudioClip level4Theme;
	[SerializeField] private AudioClip level5Theme;

	[Header("SFX"), SerializeField] private AudioClip foxDash;
	[SerializeField] private AudioClip foxDeath;
	[SerializeField] private AudioClip birdDeath;
	[SerializeField] private AudioClip lanternCollect;
	[SerializeField] private AudioClip lanternCompleted;
	[SerializeField] private AudioClip waterRockSplash;
	[SerializeField] private AudioClip waterFoxSplash;

	private Dictionary<string, AudioClip> music = new Dictionary<string, AudioClip>();
	private Dictionary<string, AudioClip> sfx = new Dictionary<string, AudioClip>();

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}

		// Add music to music dictionary
		music.Add("level1Theme", level1Theme);
		music.Add("level2Theme", level2Theme);
		music.Add("level3Theme", level3Theme);
		music.Add("level4Theme", level4Theme);
		music.Add("level5Theme", level5Theme);

		/// Add sfx to sfx dictionary
		sfx.Add("FoxDash", foxDash);
		sfx.Add("foxDeath", foxDeath);
		sfx.Add("birdDeath", birdDeath);
		sfx.Add("lanternCollect", lanternCollect);
		sfx.Add("lanternCompleted", lanternCompleted);
		sfx.Add("waterRockSplash", waterRockSplash);
		sfx.Add("waterFoxSplash", waterFoxSplash);
	}

	public void PlaySFX(string name)
	{
		// Check if sfx exists
		if (sfx.ContainsKey(name))
		{
			// Play sfx
			SfxSource.PlayOneShot(sfx[name]);
		}
	}

	public void PlayMusic(string name)
	{
		// Check if music exists
		if (music.ContainsKey(name))
		{
			// Stop if music source is playing anything
			MusicSource.Stop();

			// Play new music
			MusicSource.clip = music[name];
			MusicSource.Play();
		}
	}
}

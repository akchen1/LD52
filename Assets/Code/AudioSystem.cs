using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
	private static AudioSystem _instance;
	public static AudioSystem Instance { get { return _instance; } }

	public AudioSource SfxSource;
	public AudioSource MusicSource;

	[Header("Music"), SerializeField] private AudioClip themeA;
	[SerializeField] private AudioClip themeB;
	[SerializeField] private AudioClip themeC;
	[SerializeField] private AudioClip themeD;
	[SerializeField] private AudioClip themeDIntro;
	[SerializeField] private AudioClip themeE;

	[Header("SFX"), SerializeField] private AudioClip foxDash;
	[SerializeField] private AudioClip foxDeath;
	[SerializeField] private AudioClip birdDeath;
	[SerializeField] private AudioClip lanternCollect;
	[SerializeField] private AudioClip lanternCompleted;
	[SerializeField] private AudioClip waterRockSplash;
	[SerializeField] private AudioClip waterFoxSplash;

	[Header("General"), SerializeField] private float fadeTime;

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
		music.Add("ThemeA", themeA);
		music.Add("ThemeB", themeB);
		music.Add("ThemeC", themeC);
		music.Add("ThemeD", themeD);
		music.Add("ThemeDIntro", themeDIntro);
		music.Add("ThemeE", themeE);

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

	private IEnumerator ChangeMusic(string name)
	{
		float elapsedTime = 0f;
		float normalizedTime = 0f;

		while (elapsedTime < fadeTime)
		{
			elapsedTime += Time.deltaTime;
			normalizedTime = elapsedTime / fadeTime;

			MusicSource.volume = 1 - normalizedTime;

			yield return null;
		}

		PlayMusic(name);

		elapsedTime = 0f;
		normalizedTime = 0f;
		while (elapsedTime < fadeTime)
		{
			elapsedTime += Time.deltaTime;
			normalizedTime = elapsedTime / fadeTime;

			MusicSource.volume = normalizedTime;

			yield return null;
		}
	}

	public void PlayThemeD()
	{
		MusicSource.clip = music["ThemeDIntro"];
		MusicSource.Play();
		MusicSource.loop = false;

		StartCoroutine(PlayThemeDLoop());
	}

	private IEnumerator PlayThemeDLoop()
	{
		yield return new WaitForSeconds(music["ThemeDIntro"].length);

		MusicSource.clip = music["ThemeD"];
		MusicSource.loop = true;
		MusicSource.Play();
	}
}

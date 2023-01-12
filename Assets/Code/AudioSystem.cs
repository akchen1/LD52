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
	[SerializeField] private float musicVolume;

	[Header("SFX"), SerializeField] private AudioClip foxDash;
	[SerializeField] private AudioClip foxDeath;
	[SerializeField] private AudioClip birdDeath;
	[SerializeField] private AudioClip lanternCompleted;
	[SerializeField] private AudioClip waterRockSplash;
	[SerializeField] private AudioClip waterFoxSplash;
	[SerializeField] private AudioClip lanternCollected;
	[SerializeField] private AudioClip vineChaseStart;
	[SerializeField] private AudioClip vineChase;
	[SerializeField] private AudioClip vineEnd;
	[SerializeField] private AudioClip vineBurst;


	[Header("General"), SerializeField] private float fadeSpeed;

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
		sfx.Add("FoxDeath", foxDeath);
		sfx.Add("BirdDeath", birdDeath);
		sfx.Add("LanternCompleted", lanternCompleted);
		sfx.Add("WaterRockSplash", waterRockSplash);
		sfx.Add("WaterFoxSplash", waterFoxSplash);
		sfx.Add("LanternCollected", lanternCollected);
		sfx.Add("VineChaseStart", vineChaseStart);
		sfx.Add("VineChase", vineChase);
		sfx.Add("VineEnd", vineEnd);
		sfx.Add("VineBurst", vineBurst);
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
			MusicSource.loop = true;
		}
	}

	public IEnumerator ChangeMusic(string name)
	{
		while (MusicSource.volume > 0)
		{
			MusicSource.volume -= fadeSpeed * Time.deltaTime;
			yield return null;
		}

		PlayMusic(name);

		while (MusicSource.volume < musicVolume)
		{
			MusicSource.volume += fadeSpeed * Time.deltaTime;
			yield return null;
		}
	}

	public void PlayVineChase()
	{
		PlaySFX("VineChaseStart");

		StartCoroutine(PlayVineChaseLoop());
	}

	public void PlayVineEnd()
	{
		SfxSource.Stop();
		SfxSource.loop = false;
		PlaySFX("VineEnd");
	}

	private IEnumerator PlayVineChaseLoop()
	{
		yield return new WaitForSeconds(music["VineChaseStart"].length);

		SfxSource.clip = sfx["VineChase"];
		SfxSource.loop = true;
		SfxSource.Play();
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

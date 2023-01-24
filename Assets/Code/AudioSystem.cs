using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSystem : MonoBehaviour
{
	private static AudioSystem _instance;
	public static AudioSystem Instance { get { return _instance; } }

	public AudioSource SfxSource;
	public AudioSource MusicSource;

	public Slider SFXSlider;
	public Slider MusicSlider;

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

	private string currentMusic;

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

    private void Start()
    {
		SetVolumeLevels();
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

			currentMusic = name;
		}
	}

	public IEnumerator ChangeMusic(string name)
	{
		if (currentMusic == name) yield break;
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
		yield return new WaitForSeconds(sfx["VineChaseStart"].length);

		SfxSource.clip = sfx["VineChase"];
		SfxSource.loop = true;
		SfxSource.Play();
	}

	public void PlayThemeDIntro()
	{
		if (currentMusic == "ThemeD") return;
		MusicSource.clip = music["ThemeDIntro"];
		MusicSource.Play();
		MusicSource.loop = false;
		currentMusic = "ThemeDIntro";
	}

	public void PlayThemeDLoop()
	{
		//yield return new WaitForSeconds(music["ThemeDIntro"].length);
		if (currentMusic == "ThemeD") return;
		PlayVineChase();

		MusicSource.clip = music["ThemeD"];
		MusicSource.loop = true;
		MusicSource.Play();
		currentMusic = "ThemeD";
	}

	public void PlayThemeDEnd()
    {
		PlayVineEnd();
		StartCoroutine(ChangeMusic("ThemeE"));
    }

	private void SetVolumeLevels()
    {
		float musicLevel = PlayerPrefs.GetFloat("MusicVolume", 0.25f);
		float sfxLevel = PlayerPrefs.GetFloat("SFXVolume", 0.25f);

		musicVolume = musicLevel;
		MusicSource.volume = musicLevel;
		SfxSource.volume = sfxLevel;

		MusicSlider.value = musicLevel;
		SFXSlider.value = sfxLevel;
	}

	public void AdjustMusicVolume()
    {
        MusicSource.volume = MusicSlider.value;
        musicVolume = MusicSlider.value;
		PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
    }

	public void AdjustSFXVolume()
    {
		SfxSource.volume = SFXSlider.value;
		PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
	}

	public void ResetAudio()
    {
		currentMusic = null;
		StopAllCoroutines();
		SfxSource.Stop();
		MusicSource.Stop();
    }
}

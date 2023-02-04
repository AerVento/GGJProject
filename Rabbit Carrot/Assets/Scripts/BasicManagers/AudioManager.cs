using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using static Unity.VisualScripting.Member;

/// <summary>
/// Manager of audios in game.
/// </summary>
public class AudioManager : MonoSingleton<AudioManager>
{
    private const string PLAYERPREFS_NAME_OF_MUISC_VOLUME = "MusicVolume";
    private const string PLAYERPREFS_NAME_OF_EFFECTVOLUME = "EffectVolume";

    private AudioSource musicSource;
    /// <summary>
    /// GameObject of music audio source.
    /// </summary>
    public AudioSource MusicSource => musicSource;

    [SerializeField]
    private GameObject effectSourcePrefab;

    /// <summary>
    /// The object buffer which contained all effect sources.
    /// </summary>
    private ObjectBuffer effectSourceBuffer;

    /// <summary>
    /// The database of background music.
    /// </summary>
    [SerializeField]
    private AudioSO musicDatabase;

    /// <summary>
    /// The database of effect audio.
    /// </summary>
    [SerializeField]
    private AudioSO effectDatabase;

    /// <summary>
    /// The list of all effect source which is operating at present.
    /// </summary>
    private List<AudioSource> effectSourceList = new List<AudioSource>();


    private float musicVolume = 1;
    /// <summary>
    /// The current music volume.
    /// </summary>
    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            if (value > 1)
                musicVolume = 1;
            else if (value < 0)
                musicVolume = 0;
            else
                musicVolume = value;
            musicSource.volume = musicVolume;
            PlayerPrefs.SetFloat(PLAYERPREFS_NAME_OF_MUISC_VOLUME, musicVolume);//Serialize
        }
    }

    private float effectVolume = 1;
    /// <summary>
    /// The current effect volume.
    /// </summary>
    public float EffectVolume
    {
        get => effectVolume;
        set
        {
            if (value > 1)
                effectVolume = 1;
            else if (value < 0)
                effectVolume = 0;
            else
                effectVolume = value;
            
            foreach (var item in effectSourceList)
            {
                item.volume = effectVolume;
            }

            PlayerPrefs.SetFloat(PLAYERPREFS_NAME_OF_EFFECTVOLUME, effectVolume);//Serialize
        }
    }

    /// <summary>
    /// Initialize when activated.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        GameObject obj = new GameObject("AudioSource");

        GameObject musicSource = new GameObject("MusicSource");
        musicSource.transform.SetParent(obj.transform);
        this.musicSource = musicSource.AddComponent<AudioSource>();
        this.musicSource.loop = false;

        GameObject effectSources = new GameObject("EffectSources");
        effectSources.transform.SetParent(obj.transform);

        effectSourceBuffer = new ObjectBuffer(effectSources.transform);

        if (PlayerPrefs.HasKey(PLAYERPREFS_NAME_OF_MUISC_VOLUME))
            musicVolume = PlayerPrefs.GetFloat(PLAYERPREFS_NAME_OF_MUISC_VOLUME);
        if (PlayerPrefs.HasKey(PLAYERPREFS_NAME_OF_EFFECTVOLUME))
            effectVolume = PlayerPrefs.GetFloat(PLAYERPREFS_NAME_OF_EFFECTVOLUME);
    }


    #region BackgroundMusic
    /// <summary>
    /// Play a random background music in database.
    /// </summary>
    /// <returns>The audio clip of the music.</returns>
    public AudioClip PlayRandomBackgroundMusic()
    {
        AudioClip audio = musicDatabase.GetRandomAudio();
        if(audio != null)
        {
            musicSource.clip = audio;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
        return audio;
    }
    /// <summary>
    /// Play a music with specified name.
    /// </summary>
    /// <param name="musicName">The music name.</param>
    /// <returns>The audio clip of music.</returns>
    public AudioClip PlayBackgroundMusic(string musicName)
    {
        AudioClip audio = musicDatabase.GetAudio(musicName);
        if(audio != null)
        {
            musicSource.clip = audio;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
        return audio;
    }
    /// <summary>
    /// Play a music with given audio clip.
    /// </summary>
    /// <param name="audioClip">The audio clip of music.</param>
    /// <returns>The audio clip.</returns>
    public AudioClip PlayBackgroundMusic(AudioClip audioClip)
    {
        if(audioClip != null)
        {
            musicSource.clip = audioClip;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
        return audioClip;
    }
    /// <summary>
    /// Pause the background music.
    /// </summary>
    public void PauseBackgroundAudio()
    {
        musicSource.Pause();
    }
    /// <summary>
    /// Resume the background music.
    /// </summary>
    public void ResumeBackgroundAudio()
    {
        musicSource.UnPause();
    }
    /// <summary>
    /// Stop th background music.
    /// </summary>
    public void StopBackgroundAudio()
    {
        musicSource.Stop();
    }

    #endregion

    #region EffectMusic
    /// <summary>
    /// Whether the audio source is playing effect audio.
    /// </summary>
    /// <param name="source">The audio source,</param>
    /// <returns>True if the audio source is playing effect audio, otherwise, false.</returns>
    public bool IsPlayingEffect(AudioSource source)
    {
        return effectSourceList.Contains(source);
    }
    /// <summary>
    /// Play a effect audio with specified name.
    /// </summary>
    /// <param name="name">The name of effect audio.</param>
    /// <returns>The audio source which plays the effect audio.</returns>
    public AudioSource PlayEffectAudio(string name)
    {
        IEnumerator CollectCouroutine(AudioSource source, float time)
        {
            yield return new WaitForSeconds(time);
            effectSourceList.Remove(source);
            effectSourceBuffer.Put(effectSourcePrefab, source.gameObject);
        }
        AudioSource source = effectSourceBuffer.Get(effectSourcePrefab).GetComponent<AudioSource>();
        source.clip =  effectDatabase.GetAudio(name);
        source.volume = effectVolume;
        source.Play();
        effectSourceList.Add(source);
        MonoManager.Instance.StartCoroutine(CollectCouroutine(source, source.clip.length));
        return source;
    }
    /// <summary>
    /// Play random effect audio in the given names of effect audio.
    /// </summary>
    /// <param name="names">The names of all effect audio waiting for being randomly chosen.</param>
    /// <returns>The audio source which plays the effect audio.</returns>
    public AudioSource PlayRandomEffectAudio(params string[] names)
    {
        IEnumerator CollectCouroutine(AudioSource source, float time)
        {
            yield return new WaitForSeconds(time);
            effectSourceList.Remove(source);
            effectSourceBuffer.Put(effectSourcePrefab, source.gameObject);
        }
        if (name.Length == 0)
            return null;
        
        System.Random r = new System.Random();
        int index = r.Next(0, names.Length);

        AudioSource source = effectSourceBuffer.Get(effectSourcePrefab).GetComponent<AudioSource>();
        if(source != null)
        {
            source.clip = effectDatabase.GetAudio(names[index]);
            source.Play();
            source.volume = effectVolume;
            effectSourceList.Add(source);
            MonoManager.Instance.StartCoroutine(CollectCouroutine(source, source.clip.length));
        }

        return source;
    }
    /// <summary>
    /// Stop the effect audio of the audio source.
    /// </summary>
    /// <param name="source">The audio source.</param>
    public void StopEffectAudio(AudioSource source)
    {
        if (effectSourceList.Contains(source))
        {
            source.Stop();
            effectSourceList.Remove(source);
            effectSourceBuffer.Put(effectSourcePrefab,source.gameObject);
        }
    }
    public void StopAllEffectAudio()
    {
        foreach(var audiosource in effectSourceList)
        {
            audiosource.Stop();
            effectSourceBuffer.Put(effectSourcePrefab, audiosource.gameObject);
        }
        effectSourceList.Clear();
    }
    #endregion

    private void Start()
    {
        MusicSource.loop = true;
    }
}

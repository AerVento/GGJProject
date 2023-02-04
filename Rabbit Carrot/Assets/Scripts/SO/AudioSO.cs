using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SO database of all block tiles.
/// </summary>
[CreateAssetMenu(fileName = "AudioSO", menuName = "SO/AudioClipData")]
public class AudioSO : ScriptableObject,IEnumerable<AudioSO.Audio>
{
    [System.Serializable]
    public struct Audio
    {
        public Audio(AudioClip clip, string name)
        {
            Clip = clip;
            Name = name;
        }

        public AudioClip Clip;
        public string Name;
    }
    [SerializeField]
    private List<Audio> audioList= new List<Audio>();

    public AudioClip GetAudio(string name) => audioList.Find(x => x.Name == name).Clip;

    public AudioClip GetRandomAudio()
    {
        if(audioList.Count > 0)
            return audioList[Random.Range(0, audioList.Count)].Clip;
        else 
            return null;
    }
    public IEnumerator<Audio> GetEnumerator()
    {
        return audioList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

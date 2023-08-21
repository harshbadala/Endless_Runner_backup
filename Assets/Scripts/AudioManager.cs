using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public  Sound[] Sounds;
    void Start()
    {
        foreach(Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }

        PlaySound("MainTheme");
    }

    public void PlaySound(string name)
    {
         foreach(Sound s in Sounds)
        {
            if(s.name == name)
            {
                s.source.Play();
            }
        }
    }
    
}

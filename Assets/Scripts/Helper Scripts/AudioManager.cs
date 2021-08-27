using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource source;

    public AudioClip Shoot;
    public AudioClip reload;

    // public AudioClip Enemywalk;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play_Gunsound()
    {
        //source.clip = Shoot;
        source.PlayOneShot(Shoot);
    }
    public void Play_reloadsound()
    {
        //source.clip = Shoot;
        source.PlayOneShot(reload);
    }
}

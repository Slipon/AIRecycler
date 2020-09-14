using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCarSound : MonoBehaviour
{
    private ChuckSubInstance chuck;

    void Start()
    {
        chuck = GetComponent<ChuckSubInstance>();
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "CarOut")
        {
            chuck.RunCode(@"
                SndBuf buf => dac;
                me.dir() + ""../ProjetoFinal/AudioSamples/horn.wav"" => buf.read;
                0 => buf.pos;
                1 => buf.rate;
                buf.length()/buf.rate() => now;
            ");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Misc
{
    public class MixControls : MonoBehaviour
    {
        public AudioMixer masterMixer; 

        public void SetSoundVol(float vol)
        {
            masterMixer.SetFloat("masterVol", vol);
        }

    }
}

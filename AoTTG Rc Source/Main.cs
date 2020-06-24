using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public bool[] Anim = new bool[5];
    List<string> ee = new List<string>();
    public static Main instance;
    public bool Animateion;
    public FengGameManagerMKII Feng;
    public bool Fade;

    public string Animate(string name)
    {
        string[] hexcode = ee.ToArray();
        int index = 0, position = 0;
        for (int i = 0; i < hexcode.Length; i++)
        {
            string sub = name.Substring(position);
            if (sub.Contains(hexcode[i]))
            {
                index = name.IndexOf(hexcode[i], position);
            }
            else if (name.Contains(hexcode[i]))
            {
                index = name.IndexOf(hexcode[i]);
            }
            name = name.Remove(index, 6);
            if (i > 0)
            {
                name = name.Insert(index, hexcode[i - 1]);
            }
            else
            {
                name = name.Insert(index, hexcode[hexcode.Length - 1]);
            }
            position = index + 7;
        }
        return name;
    }

    System.Collections.IEnumerator Linear()
    {
        while (Anim[1])
        {
            Main.instance.GrabHex(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name].ToString());
            yield return new WaitForSeconds(.025f);
            name = Main.instance.Animate(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name].ToString());
            yield return new WaitForSeconds(.025f);
        }
    }

    void LateUpdate()
    {
        if (Anim[0] && !Anim[1])
        {
            Anim[1] = true;
            base.StartCoroutine(Linear());
        }
        else if ((Anim[0] && Anim[1]) || (!Anim[0] && Anim[1]))
        {
            Anim[1] = false;
            if (!string.IsNullOrEmpty(name))
            {
                ExitGames.Client.Photon.Hashtable Hash = new ExitGames.Client.Photon.Hashtable();
                Hash.Add(PhotonPlayerProperty.name, name);
                PhotonNetwork.player.SetCustomProperties(Hash);
            }
        }
        if (Anim[2] && !Anim[3])
        {
            Anim[3] = true; 
            //base.StartCoroutine(Generate());
        }
        else if ((Anim[2] && Anim[3]) || (!Anim[2] && Anim[3]))
        {
            Anim[3] = false;
            if (!string.IsNullOrEmpty(name))
            {
                ExitGames.Client.Photon.Hashtable Hash = new ExitGames.Client.Photon.Hashtable();
                Hash.Add(PhotonPlayerProperty.name, name);
                PhotonNetwork.player.SetCustomProperties(Hash);
            }
        }
    }

    public void GrabHex(string name)
    {
        ee.Clear();
        while (name.Contains("["))
        {
            int index = name.IndexOf("[");
            string hex = name.Substring(index + 1, 6);
            ee.Add(hex);
            name = name.Remove(index, 8);
        }
    }
    
    public IEnumerator changeName()
    {
        string[] colors = new string[] { "[222222]", "[e945fb]", "[222222]", "[32f8a0]", "[222222]", "[2cecff]", "[222222]", "[f95790]", "[222222]", "[7b2eff]" };
        while (Fade)
        {
            foreach (string color in colors)
            {
                ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
                hash.Add(PhotonPlayerProperty.name, color + LoginFengKAI.player.name.StripHex());
                PhotonNetwork.player.SetCustomProperties(hash);
                yield return new WaitForSeconds(.2f);
                if (!Fade)
                {
                    yield break;
                }
            }
        }
    }
}

using UnityEngine;
using System.Linq;

public class GeneralAudioController : MonoBehaviour
{
    public GameObject[] AudioObjects;

    public void PlaySoundInstance(string clipName)
    {
        var sourceObject =
            AudioObjects.Select(x => new {Obj = x, Clip = x.GetComponent<AudioSource>().clip.name})
                .FirstOrDefault(x => x.Clip == clipName);

        if (sourceObject == null)
            return;

        Instantiate(sourceObject.Obj, new Vector3(), Quaternion.identity);
    }

    public static void PlaySound(string clipName)
    {
        var controller = GameObject.Find("GeneralAudioController");
        if (controller == null)
            return;

        controller.GetComponent<GeneralAudioController>().PlaySoundInstance(clipName);
    }
}

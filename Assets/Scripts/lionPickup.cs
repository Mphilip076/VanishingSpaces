using UnityEngine;

public class LionPickup : PickableItem
{
    [Header("Identity")]
    public string lionID; // Give each lion a unique ID in Inspector

    void Start()
    {
        if (PlayerPrefs.GetInt(lionID, 0) == 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    public override void OnPickup(Transform holdPosition)
    {
        base.OnPickup(holdPosition);

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
            audio.Stop();

        LionWhisper whisper = GetComponent<LionWhisper>();
        if (whisper != null)
            whisper.enabled = false;

        PlayerPrefs.SetInt(lionID, 1);
        PlayerPrefs.Save();

        Destroy(gameObject);
    }
}
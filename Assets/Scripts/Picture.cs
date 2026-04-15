using UnityEngine;

public class Picture : PickableItem
{
    [Header("Pictures")]
    public static Picture A = null;
    public static Picture B = null;
    public static Picture C = null;

    public override void Start()
    {      
        base.Start();  

        // Prevent duplicates
        if(this.name == "Picture A" && A == null)
        {
            Debug.Log("[Picture] Assigned Picture A");
            A = this;
            DontDestroyOnLoad(this);
        }
        else if(this.name == "Picture B" && B == null)
        {
            Debug.Log("[Picture] Assigned Picture B");
            B = this;
            DontDestroyOnLoad(this);
        }
        else if(this.name == "Picture C" && C == null)
        {
            Debug.Log("[Picture] Assigned Picture C");
            C = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("[Picture] Duplicate picture found, destroying");
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class BathroomPuzzle : MonoBehaviour
{

    [Header("Puzzle Settings")]
    public int correctToiletPosition;

    

    private static BathroomPuzzle instance = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPuzzleCompletion();
    }

    void CheckPuzzleCompletion()
    {
        if(!FallenVase.isUpright) return;
        if(ToiletLid.GetPosition() != correctToiletPosition) return;

    }
}

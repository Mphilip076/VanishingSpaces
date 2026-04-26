using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;

public class RollTower : MonoBehaviour
{
    [Header("Rolls")]
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;
    public GameObject E;

    public static volatile bool trigger;
    private static RollTower instance = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        ModifyTower();
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger){ 
            ModifyTower();
            trigger = false;
        }
    }

    public void ModifyTower()
    {
        if(ToiletRoll.rollsMoved == 0)
        {
            A.SetActive(false);
            B.SetActive(false);
            C.SetActive(false);
            D.SetActive(false);
            E.SetActive(false);
        }
        else if(ToiletRoll.rollsMoved == 1)
        {
            A.SetActive(true);
            B.SetActive(false);
            C.SetActive(false);
            D.SetActive(false);
            E.SetActive(false);
        }
        else if(ToiletRoll.rollsMoved == 2)
        {
            A.SetActive(true);
            B.SetActive(true);
            C.SetActive(false);
            D.SetActive(false);
            E.SetActive(false);
        }
        else if(ToiletRoll.rollsMoved == 3)
        {
            A.SetActive(true);
            B.SetActive(true);
            C.SetActive(true);
            D.SetActive(false);
            E.SetActive(false);
        }
        else if(ToiletRoll.rollsMoved == 4)
        {
            A.SetActive(true);
            B.SetActive(true);
            C.SetActive(true);
            D.SetActive(true);
            E.SetActive(false);
        }
        else if(ToiletRoll.rollsMoved == 5)
        {
            A.SetActive(true);
            B.SetActive(true);
            C.SetActive(true);
            D.SetActive(true);
            E.SetActive(true);
        }
    }
}

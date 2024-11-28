using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;
    public int chosenSkinId;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    public void SetSkinId(int id)
    {
        chosenSkinId = id;
    }
    public int GetSkinId()
    {
        return chosenSkinId;
    }
}

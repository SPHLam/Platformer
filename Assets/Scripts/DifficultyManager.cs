using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DifficultyType
{
    Easy = 1, Normal, Hard
}

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }
    public DifficultyType difficulty;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }
    public void SetDifficulty(DifficultyType newDifficulty)
    {
        difficulty = newDifficulty;
    }

    public void LoadDifficulty(int difficultyIndex)
    {
        difficulty = (DifficultyType) difficultyIndex;
    }
}

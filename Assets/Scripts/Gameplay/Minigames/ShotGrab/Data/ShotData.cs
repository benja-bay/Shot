using UnityEngine;

namespace Gameplay.Minigames.ShotGrab.Data
{
    [CreateAssetMenu(menuName = "Shot!/Shot Data")]
    public class ShotData : ScriptableObject
    {
        [Header("Info")]
        public string shotName;

        [Header("Visual")]
        public GameObject visualPrefab;

        [Header("Score")]
        public int points = 1;
    }
}
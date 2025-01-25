using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "GameEventSO", menuName = "DSC/Game Event SO")]
    public class GameEventSO : ScriptableObject
    {
        public void GameOver()
        {
            GameManager.GameOver();
        }
    }
}

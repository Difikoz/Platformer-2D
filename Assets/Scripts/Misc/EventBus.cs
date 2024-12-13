using System;
using UnityEngine;

namespace WinterUniverse
{
    public static class EventBus
    {
        public static Action<bool> OnGamePaused;

        public static void GamePaused(bool paused)
        {
            OnGamePaused?.Invoke(paused);
        }
    }
}
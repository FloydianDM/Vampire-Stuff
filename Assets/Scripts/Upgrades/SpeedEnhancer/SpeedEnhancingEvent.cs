using System;
using UnityEngine;

public class SpeedEnhancingEvent : MonoBehaviour
{
   public event Action<SpeedEnhancingEvent, SpeedEnhancingEventArgs> OnSpeedEnhancing;

   public void CallSpeedEnhancingEvent(float speedModifier)
   {
      OnSpeedEnhancing?.Invoke(this, new SpeedEnhancingEventArgs()
      {
         SpeedModifierValue = speedModifier
      });
   }
}

public class SpeedEnhancingEventArgs : EventArgs
{
   public float SpeedModifierValue;
}
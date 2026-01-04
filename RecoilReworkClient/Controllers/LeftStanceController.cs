using EFT.Animations;
using UnityEngine;

namespace RecoilReworkClient.Controllers
{
    public class LeftStanceController : MonoBehaviour
    {
        public static LeftStanceController Instance { get; private set; }

        public bool IsLeftStance { get; private set; } = false;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            
            Instance = this;
        }

        public void ToggleLeftStance()
        {
            IsLeftStance = !IsLeftStance;
        }
        
        public void ApplyComplexRotation(ProceduralWeaponAnimation pwa)
        {
            
        }
    }
}

using UnityEngine;

namespace Assets.Scripts.DeviceScripts
{
    public class Lamp : Device
    {
        [Header("Материал включенной лампы")]
        [SerializeField] private Material matOn;
 
        [Header("Материал выключенной лампы")]
        [SerializeField] private Material matOff;

        private Renderer renderer;

        public override void Work(float signal)
        {
            if (signal == 1)
            {
                renderer.material = matOn;
            }
            else
            {
                renderer.material = matOff;
            }
        }

        protected override void InitialSettings()
        {
            renderer = GetComponent<Renderer>();
            renderer.material = matOff;
        }
    }


}

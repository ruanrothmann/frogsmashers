using UnityEngine;
using System.Collections;
namespace FreeLives
{
    public class TestCube : MonoBehaviour
    {

        public InputReader.Device device;

        InputState inputState = new InputState();

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            InputReader.GetInput(device, inputState);
            transform.Translate(inputState.xAxis * 10f * Time.deltaTime, inputState.yAxis * 10f * Time.deltaTime, 0f, Space.World);

            if (inputState.aButton && !inputState.wasAButton)
            {
                SoundController.PlaySoundEffect("Test");
            }
            if (inputState.bButton && !inputState.wasBButton)
            {
                SoundController.PlaySoundEffect("Test", 0.5f, transform.position);
            }

            if (inputState.xButton && !inputState.wasXButton)
            {
                CameraShake.Instance.Shake(1f, 5f);
            }

            if (inputState.yButton && !inputState.wasYButton)
            {
                CameraShake.Instance.Vibrate(1f);
            }
        }
    }
}
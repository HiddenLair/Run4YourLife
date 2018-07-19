using UnityEngine;
using UnityEngine.Events;

namespace Run4YourLife.InputManagement
{
    public class OnButtonPressedControlScheme : ControlScheme
    {
        [SerializeField]
        private string buttonId;

        [SerializeField]
        protected UnityEvent onButtonPressed;

        [SerializeField]
        protected UnityEvent onButtonReleased;

        public InputAction InputAction { get; private set; }

        void Awake()
        {
            InputActions.Add(InputAction = new InputAction(new InputSource(GetButton()), "OneButtonControlScheme::InputActions"));
        }

        void Start()
        {
            InputDevice = InputDeviceManager.Instance.DefaultInputDevice;
            Active = true;
        }

        protected virtual void Update()
        {
            if(InputAction.Started())
            {
                OnPressed();
            }

            if(InputAction.Ended())
            {
                OnReleased();
            }
        }

        protected virtual void OnPressed()
        {
            onButtonPressed.Invoke();
        }

        protected virtual void OnReleased()
        {
            onButtonReleased.Invoke();
        }

        private Button GetButton()
        {
            Button button = null;

            foreach(Button currentButton in Button.BUTTONS)
            {
                if(currentButton.ID == buttonId)
                {
                    button = currentButton;
                    break;
                }
            }

            Debug.Assert(button != null, "OnButtonPressedControlScheme: buttonId does not exist!");

            return button;
        }
    }
}
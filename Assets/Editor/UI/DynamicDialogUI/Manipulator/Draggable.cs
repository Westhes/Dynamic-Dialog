namespace GameName.UI.Manipulators
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public class Draggable : MouseManipulator
    {
        private bool isActive = false;
        private Vector2 mouseStart;

        public Draggable()
        {
            activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse, });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (isActive)
            {
                evt.StopImmediatePropagation();
                return;
            }

            if (CanStartManipulation(evt))
            {
                mouseStart = evt.localMousePosition;
                isActive = true;
                target.CaptureMouse();
                evt.StopPropagation();
            }
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (!isActive || !target.HasMouseCapture())
                return;

            Vector2 diff = evt.localMousePosition - mouseStart;
            //target.style.top = Mathf.Clamp(target.layout.y + diff.y, 0 + height, 900);
            //target.style.left = Mathf.Clamp(target.layout.x + diff.x, 0 + width, )
            target.style.top = target.layout.y + diff.y;  //Mathf.Clamp(target.layout.y + diff.y, -target.layout.height / 2, 0);
            target.style.left = target.layout.x + diff.x; //Mathf.Clamp(target.layout.x + diff.x, -target.layout.width / 2, 0);

            evt.StopPropagation();
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (!isActive || !target.HasMouseCapture() || !CanStopManipulation(evt))
                return;

            isActive = false;
            target.ReleaseMouse();
            evt.StopPropagation();
        }
    }
}
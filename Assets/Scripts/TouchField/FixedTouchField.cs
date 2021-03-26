using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
      public float sensitivity = 1;
      private Vector2 touchDist;
      private Vector2 pointerOld;
      private int pointerId;
      private bool pressed;

      public Vector2 Movement
      {
            get {return touchDist;}
      }

      void Update()
      {
            if (pressed)
            {
                  if (pointerId >= 0 && pointerId < Input.touches.Length)
                  {
                        touchDist = Input.touches[pointerId].position - pointerOld;
                        pointerOld = Input.touches[pointerId].position;
                        touchDist *= sensitivity;
                  }
                  else
                  {
                        touchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - pointerOld;
                        pointerOld = Input.mousePosition;
                        touchDist.Normalize();
                        touchDist *= sensitivity;
                  }
            }
            else
            {
                  touchDist = new Vector2();
            }
      }

      public void OnPointerDown(PointerEventData eventData)
      {
            pressed = true;
            pointerId = eventData.pointerId;
            pointerOld = eventData.position;
      }


      public void OnPointerUp(PointerEventData eventData)
      {
            pressed = false;
      }
}
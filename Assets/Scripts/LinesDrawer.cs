using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class LinesDrawer : MonoBehaviour
{
   [SerializeField] private UserInput userInput;
   
   [SerializeField] private int interactableLayerIndex;
   
   private Line currentLine;
   private Route currentRoute;

   private RaycastDetector raycastDetector = new();

   public UnityAction<Route, List<Vector3>> OnParkLinkedToLine;
   public UnityAction<Route> OnBeginDraw;
   public UnityAction OnDraw;
   public UnityAction OnEndDraw;

   private void Start()
   {
      userInput.OnMouseDown += OnMouseDownHandler;
      userInput.OnMouseMove += OnMouseMoveHandler;
      userInput.OnMouseUp += OnMouseUPHandler;
   }

   private void OnMouseDownHandler()
   {
      ContactInfo contactInfo = raycastDetector.RayCast(interactableLayerIndex);
      if (contactInfo.contacted)
      {
         bool isShip = contactInfo.collider.TryGetComponent(out Ship _ship);
         Debug.Log(isShip);
         if (isShip && _ship.route.isActive)
         {
            Debug.Log("asd");
            currentRoute = _ship.route;
            currentLine = currentRoute.line;
            currentLine.Init();
            OnBeginDraw?.Invoke(currentRoute);
         }
      }
   }

   private void OnMouseMoveHandler()
   {
      if (currentRoute!=null)
      {
         Debug.Log("qwe");
         ContactInfo contactInfo = raycastDetector.RayCast(interactableLayerIndex);
         if (contactInfo.contacted)
         {
            Vector3 newPoint = contactInfo.point;
            if (currentLine.length>=currentRoute.maxLineLength)
            {
               currentLine.Clear();
               OnMouseUPHandler();
               return;
            }
            currentLine.AddPoint(newPoint);
            OnDraw?.Invoke();
            bool isPark = contactInfo.collider.TryGetComponent(out Park _park);

            if (isPark)
            {
               Route parkRoute = _park.route;
               if (parkRoute ==currentRoute)
               {
                  currentLine.AddPoint(contactInfo.transform.position);
                  OnDraw?.Invoke();

               }
               else
               {
                  //delete the line
                  currentLine.Clear();
               }
               OnMouseUPHandler();
            }
         }
      }
   }

   private void OnMouseUPHandler()
   {
      if (currentRoute != null)
      {
         ContactInfo contactInfo = raycastDetector.RayCast(interactableLayerIndex);

         if (contactInfo.contacted)
         {
            bool isPark  = contactInfo.collider.TryGetComponent(out Park _park);

            if (currentLine.pointsCount<2 || !isPark )
            {
               currentLine.Clear();
            }
            else
            {
               OnParkLinkedToLine?.Invoke(currentRoute,currentLine.points);
               currentRoute.Disactivate();
            }
            
         }
         else
         {
            currentLine.Clear();
         }
      }

      ResetDrawer();
      OnEndDraw?.Invoke();

   }

   private void ResetDrawer()
   {
      currentRoute = null;
      currentLine = null;
      
   }
}

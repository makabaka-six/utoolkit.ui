using System;
using System.Collections.Generic;
using UnityEngine;

namespace UToolkit.UI
{
    public class UIKit
    {
        private static Stack<Controller> controllers = new Stack<Controller>();

        public static Func<string, GameObject> loadViewFunc;

        public static T Open<T>(Intent intent = null) where T : Controller, new()
        {
            string viewName = typeof(T).Name.Replace("Controller", "View");
            GameObject entity = loadViewFunc?.Invoke(viewName);

            if (entity == null)
            {
                Debug.LogError($"Failed to load view: {viewName}");
                return null;
            }

            T controller = new T();
            controller.OnCreate(intent);
            controllers.Push(new T());
            return controller;
        }

        public static void Show<T>() where T : Controller
        {
            // TODO:显示视图
        }

        public static void Hide<T>() where T : Controller
        {
            // TODO:隐藏视图
        }

        public static void Close<T>() where T : Controller
        {
            // TODO:关闭视图
        }
    }
}
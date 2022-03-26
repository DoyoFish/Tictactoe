using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Doyo.UnityFramework
{
    public static class GameExtensions
    {
        public static string GetRootPath(this Transform child)
        {
            string path = child.name;
            Transform parent = child.parent;
            while (parent)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
            return path;
        }

        public static Vector3 AddX(this Vector3 vector, float x)
        {
            vector.x += x;
            return vector;
        }

        public static Vector3 AddY(this Vector3 vector, float y)
        {
            vector.y += y;
            return vector;
        }

        public static Vector3 AddZ(this Vector3 vector, float z)
        {
            vector.z += z;
            return vector;
        }

        public static Vector3 SetX(this Vector3 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        public static Vector3 SetY(this Vector3 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        public static Vector3 SetZ(this Vector3 vector, float z)
        {
            vector.z = z;
            return vector;
        }

        public static Vector2 AddX(this Vector2 vector, float x)
        {
            vector.x += x;
            return vector;
        }

        public static Vector2 AddY(this Vector2 vector, float y)
        {
            vector.y += y;
            return vector;
        }

        public static Vector2 SetX(this Vector2 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        public static Vector2 SetY(this Vector2 vector, float y)
        {
            vector.y = y;
            return vector;
        }
    }
}

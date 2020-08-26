using JetBrains.Annotations;
using UnityEngine;

namespace OsaVR.Utils
{
    public static class CommonExtensions
    {
        [CanBeNull]
        public static GameObject FindChildNamed(this GameObject o, string name)
        {
            foreach (Transform child in o.transform)
            {
                if (child.gameObject.name.Equals(name))
                {
                    return child.gameObject;
                }

                GameObject result = child.gameObject.FindChildNamed(name);
                if (result)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace System.Rendering
{
    public static class Activating
    {
        public static T CreateInstance<T>(params object[] args)
        {
            return CreateInstanceOfSubclass<T>(typeof(T), args);
        }

        public static T CreateInstanceOfSubclass<T>(Type childType, params object[] args)
        {
            return (T)Activator.CreateInstance(childType, Reflection.BindingFlags.NonPublic | Reflection.BindingFlags.Public | Reflection.BindingFlags.Instance, Type.DefaultBinder, args, null);
        }


    }
}

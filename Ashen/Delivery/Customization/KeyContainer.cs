using UnityEngine;
using System.Collections;
using System;

namespace Ashen.DeliverySystem
{
    public struct KeyContainer<T>
    {
        public T source;
        public string key;

        public KeyContainer(T source, string key)
        {
            this.source = source;
            this.key = key;
        }

        public KeyContainer(T source)
        {
            this.source = source;
            long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.key = "" + milliseconds;
        }
    }
}
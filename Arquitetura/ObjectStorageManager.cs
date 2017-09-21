using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Arquitetura
{
    internal class ObjectStorageManager<TType> where TType : class
    {
        private readonly IDictionary<int, List<KeyValuePair<string, TType>>> objects = new Dictionary<int, List<KeyValuePair<string, TType>>>();

        public int ThreadID
        {
            get { return Thread.CurrentThread.ManagedThreadId; }
        }

        public IList<KeyValuePair<string, TType>> CurrentThreadObjects
        {
            get
            {
                if (this.objects.Count == 0 || !objects.ContainsKey(ThreadID))
                    return null;
                return objects[ThreadID];
            }
        }

        public KeyValuePair<string, TType>? GetKeyValue(string typeName)
        {
            var storedObjects = CurrentThreadObjects;
            if (storedObjects == null || storedObjects.Count == 0)
                return null;

            if (!storedObjects.Any(kv => kv.Key == typeName))
                return null;

            return storedObjects.Single(kv => kv.Key == typeName);
        }

        public TType GetObject(string typeName)
        {
            var kv = GetKeyValue(typeName);
            if (kv == null)
                return null;

            return kv.GetValueOrDefault().Value;
        }

        internal void Add(string typeName, TType theObject, bool allowDuplication = false)
        {
            if (!allowDuplication && GetObject(typeName) != null)
                throw new InvalidOperationException("[ObjectStorageManager<" + typeof(TType).Name + ">]Object for " + typeName + " already created and duplication is not allowed");

            var kvObject = new KeyValuePair<string, TType>(typeName, theObject);

            if (CurrentThreadObjects == null)
                objects[ThreadID] = new List<KeyValuePair<string, TType>>();

            CurrentThreadObjects.Add(kvObject);
        }

        public void Remove(string typeName)
        {
            if (CurrentThreadObjects == null)
                return;

            var kv = GetKeyValue(typeName);
            if (kv == null)
                return;

            CurrentThreadObjects.Remove(kv.Value);

            if (!CurrentThreadObjects.Any())
                this.objects.Remove(ThreadID);
        }

        public void RemoveThread()
        {
            if (CurrentThreadObjects == null)
                return;

            this.objects.Remove(ThreadID);
        }

    }
}

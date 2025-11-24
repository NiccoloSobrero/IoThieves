using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
	public class ObjectPoolingManager
	{
		private static volatile ObjectPoolingManager _instance;

		//look up list of various object pools.
		private readonly Dictionary<String, ObjectPool> _objectPools;

		//object for locking
		private static readonly object SyncRoot = new();

		private ObjectPoolingManager()
		{
			_objectPools = new Dictionary<String, ObjectPool>();
		}

		public static ObjectPoolingManager Instance
		{
			get
			{
				if (_instance == null)
				{
					//lock access, if it is already locked, wait.
					lock (SyncRoot)
					{
						if (_instance == null)
						{
							_instance = new ObjectPoolingManager();
						}
					}
				}
				
				return _instance;
			}
		}

		public bool CreatePool(GameObject objToPool, Transform parent, int initialPoolSize, int maxPoolSize)
		{
			if (Instance._objectPools.ContainsKey(objToPool.name))
			{
				return false;
			}

			ObjectPool nPool = new ObjectPool(objToPool, parent, initialPoolSize, maxPoolSize);
				
			Instance._objectPools.Add(objToPool.name, nPool);
				
			return true;
		}

		public GameObject GetObject(string objName)
		{
			return Instance._objectPools[objName].GetObject();
		}
	}
}

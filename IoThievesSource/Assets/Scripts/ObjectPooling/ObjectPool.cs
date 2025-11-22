using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
	public class ObjectPool
	{
		private readonly List<GameObject> _pooledObjects;

		private readonly GameObject _pooledObj;
		private readonly Transform _parent;

		private readonly int _maxPoolSize;

		public ObjectPool(GameObject obj, Transform parent, int initialPoolSize, int maxPoolSize)
		{
			if (parent == null)
			{
				parent = new GameObject().transform;
				parent.gameObject.name = obj.name;
				parent.position = Vector3.zero;
				parent.rotation = Quaternion.identity;
				Object.DontDestroyOnLoad (parent.gameObject);
			}

			_pooledObjects = new List<GameObject>();

			// Fill pool with #initialPoolSize objects
			for (int i = 0; i < initialPoolSize; i++)
			{
				GameObject newObj = Object.Instantiate(obj, Vector3.zero, Quaternion.identity, parent);

				newObj.SetActive(false);

				_pooledObjects.Add(newObj);
			}

			_maxPoolSize = maxPoolSize;
			_pooledObj = obj;
			_parent = parent;
		}

		public GameObject GetObject()
		{
			for (int i = 0; i < _pooledObjects.Count; i++)
			{
				if (_pooledObjects[i].activeSelf == false)
				{
					_pooledObjects[i].SetActive(true);
					
					return _pooledObjects[i];
				}
			}
			
			if (_maxPoolSize > _pooledObjects.Count)
			{
				GameObject newObj = Object.Instantiate(_pooledObj, Vector3.zero, Quaternion.identity, _parent);
				
				newObj.SetActive(true);
				
				_pooledObjects.Add(newObj);
				
				return newObj;
			}
			
			return null;
		}
	}
}

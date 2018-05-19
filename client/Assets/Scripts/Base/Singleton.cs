using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uppower
{
	public class Singleton<T> where T: class, new()
	{
		T _instance = null;
		public T Instance {
			get {
				if (_instance == null) {
					_instance = new T();
				}
				return _instance;
			}
		}
	}
}

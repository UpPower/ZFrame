using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Uppower 
{
	public interface IManager
	{
		void OnInit();
		void OnDestroy();
		void OnUpdate(float dt);
	}
}
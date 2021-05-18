﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	public class CombineRuntime : MonoBehaviour
	{
		public MeshCombiner meshCombiner;
		public bool useSearchConditions = true;
		//public GameObject[] gos;

		public void Combine()
		{
			//meshCombiner.searchOptions.parentGOs = gos;
			meshCombiner.CombineAll(useSearchConditions);
		}
	}
}

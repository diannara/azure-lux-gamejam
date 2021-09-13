using UnityEngine;

namespace Diannara.ScriptableObjects.Variables
{
	public class VariableSO : ScriptableObject
	{
		public string Name;

		[TextArea] public string DeveloperDescription;
	}
}

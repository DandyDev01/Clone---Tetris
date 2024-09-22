using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class MoveComponentToParent : MonoBehaviour
{
	private const string moveToParent = "CONTEXT/Component/Move To Parent";

	[MenuItem(moveToParent, priority = 501)]
	public static void MoveComponentToParentMenuItem(MenuCommand command)
	{
		Component sourceComponent = command.context as Component;
		
		Transform parent = sourceComponent.gameObject.transform.parent;

		if (!ComponentUtility.CopyComponent(sourceComponent) ||
			!ComponentUtility.PasteComponentAsNew(parent.gameObject))
		{
			Debug.Log("Cannot move component to parent", sourceComponent.gameObject);
			return;
		}
	
		Undo.DestroyObjectImmediate(sourceComponent);
	}

	[MenuItem(moveToParent, validate = true)]
	public static bool MoveComponentToParentMenuItemValidation(MenuCommand command)
	{
		Component component = command.context as Component;

		if (component.gameObject.transform.parent == null)
			return false;

		return true;
	}
}

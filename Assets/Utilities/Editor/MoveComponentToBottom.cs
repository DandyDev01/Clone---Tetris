using UnityEngine;
using UnityEditor;


public class MoveComponentToBottom
{
	private const string moveToBottom = "CONTEXT/Component/Move To Bottom";

	[MenuItem(moveToBottom, priority = 503)]
	public static void MoveComponentToBottomMenuItem(MenuCommand command)
	{
		while (UnityEditorInternal.ComponentUtility.MoveComponentDown((Component)command.context)) ;
	}

	[MenuItem(moveToBottom, validate = true)]
	public static bool MoveComponentToBottomMenuItemValidation(MenuCommand command)
	{
		Component[] components = ((Component)command.context).gameObject.GetComponents<Component>();

		for (int i = 0; i < components.Length; i++)
		{
			if (components[i] == ((Component)command.context) && i == components.Length -1)
				return false;
		}

		return true;
	}
}

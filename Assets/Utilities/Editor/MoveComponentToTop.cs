using UnityEngine;
using UnityEditor;


public class MoveComponentToTop
{
    private const string moveToTop = "CONTEXT/Component/Move To Top";

	[MenuItem(moveToTop, priority=502)]    
    public static void MoveComponentToTopMenuItem(MenuCommand command)
    {
        while (UnityEditorInternal.ComponentUtility.MoveComponentUp((Component)command.context));
    }

    [MenuItem(moveToTop, validate = true)]
    public static bool MoveComponentToTopMenuItemValidation(MenuCommand command)
    {
        Component[] components = ((Component)command.context).gameObject.GetComponents<Component>();

        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == ((Component)command.context) && i == 1)
                return false;
        }

        return true;
    }
}

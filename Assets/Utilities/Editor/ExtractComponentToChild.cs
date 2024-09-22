using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class ExtractComponentToChild
{
	[MenuItem("CONTEXT/Component/Extract", priority=504)]
	public static void ExtractMenuOption(MenuCommand command)
	{
		Component sourceComponent = command.context as Component;

		int undoGroupIndex = Undo.GetCurrentGroup();
		Undo.IncrementCurrentGroup();

		GameObject gameObject = new GameObject(sourceComponent.GetType().Name);
		gameObject.transform.parent = sourceComponent.transform;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;

		Undo.RegisterCreatedObjectUndo(gameObject, "Created child object");

		if (!ComponentUtility.CopyComponent(sourceComponent) ||
			!ComponentUtility.PasteComponentAsNew(gameObject))
		{
			Debug.LogError("Connot extrat component", sourceComponent.gameObject);

			Undo.CollapseUndoOperations(undoGroupIndex);
			Undo.PerformUndo();
			return;
		}

		Undo.DestroyObjectImmediate(sourceComponent);

		Undo.CollapseUndoOperations(undoGroupIndex);
	}
}

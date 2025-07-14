#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VehicleCombatBehaviourSystem))]
public class VehicleCombatSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var controller = (VehicleCombatBehaviourSystem)target;

        EditorGUI.BeginChangeCheck();

        controller.useAbordagingController = EditorGUILayout.Toggle("Abordage Controller", controller.useAbordagingController);
        controller.useChasingController = EditorGUILayout.Toggle("Chase Controller", controller.useChasingController);
        controller.useEscapingController = EditorGUILayout.Toggle("Escape Controller", controller.useEscapingController);
        controller.useNoDriverController = EditorGUILayout.Toggle("No Driver Controller", controller.useNoDriverController);

        if (EditorGUI.EndChangeCheck())
        {
            ManageComponent<AbordagingVehicleController>(controller, controller.useAbordagingController, out controller.abordagingVehicleController);
            ManageComponent<ChasingVehicleController>(controller, controller.useChasingController, out controller.chasingVehicleController);
            ManageComponent<EscapingVehicleController>(controller, controller.useEscapingController, out controller.escapingVehicleController);
            ManageComponent<NoDriverVehicleController>(controller, controller.useNoDriverController, out controller.noDriverVehicleController);


            EditorUtility.SetDirty(controller);
        }

        DrawDefaultInspector(); // optional
    }

    private void ManageComponent<T>(VehicleCombatBehaviourSystem target, bool enabled, out T controller) where T : VehicleBehaviourController
    {
        controller = target.GetComponent<T>();
        if (enabled)
        {
            if (controller == null)
            {
                controller = target.gameObject.AddComponent<T>();
                Debug.Log(target);
                controller.Controller = target;
                controller.enabled = true;
            }
        }
        else
        {
            if (controller != null)
            {
                DestroyImmediate(controller);
                controller = null;
            }
        }
    }
}
#endif
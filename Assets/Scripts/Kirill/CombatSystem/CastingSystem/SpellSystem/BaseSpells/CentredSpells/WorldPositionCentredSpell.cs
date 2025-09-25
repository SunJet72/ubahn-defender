using Fusion;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldPositionCentredSpell", menuName = "Scriptable Objects/WorldPositionCentredSpell")]
public class WorldPositionCentredSpell : ActiveSpell
{
    [SerializeField] private ActiveSpellData data;
    [SerializeField] private NetworkObject emptyNO;

    public override SpellData SpellData => throw new System.NotImplementedException();

    protected override Transform GetStartTransform(UnitController caster, Vector2 end)
    {
        NetworkObject no = caster.Runner.Spawn(
            emptyNO,
            new Vector3(end.x, end.y, 0f),
            Quaternion.identity,
            null
        );
        return no.transform;
    }
}

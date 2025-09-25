using Fusion;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileCentredSpell", menuName = "Scriptable Objects/ProjectileCentredSpell")]
public class ProjectileCentredSpell : ActiveSpell
{
    [SerializeField] private ProjectileCentredSpellData data;
    public override SpellData SpellData => data;

    protected override Transform GetStartTransform(UnitController caster, Vector2 end)
    {
        NetworkObject no = caster.Runner.Spawn(
            data._projectilePrefab,
            caster.transform.position,
            Quaternion.identity,
            null
        );
        no.GetComponent<Projectile>().Init(caster, end);
        return no.transform;
    }
}

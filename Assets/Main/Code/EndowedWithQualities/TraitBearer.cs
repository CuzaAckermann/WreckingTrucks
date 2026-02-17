using System.Collections.Generic;

public class TraitBearer
{
    private readonly List<ITrait> _traits;

    public TraitBearer()
    {
        _traits = new List<ITrait>();
    }

    public void AddTrait(ITrait trait)
    {
        if (Validator.IsContains(_traits, trait))
        {
            return;
        }

        _traits.Add(trait);
    }

    public void RemoveTrait(ITrait trait)
    {
        if (Validator.IsContains(_traits, trait) == false)
        {
            return;
        }

        _traits.Remove(trait);
    }

    public bool TryGetTrait<T>(out T trait) where T : ITrait
    {
        trait = default;

        for (int currentTrait = 0; currentTrait < _traits.Count; currentTrait++)
        {
            if (Validator.IsRequiredType(_traits[currentTrait], out T requiredTrait))
            {
                trait = requiredTrait;

                break;
            }
        }

        return trait != null;
    }
}
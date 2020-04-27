module StatCalculator

open Models

let addStats (x : ItemStats) (y : ItemStats) =
    {
        Strength    = x.Strength + y.Strength
        Agility     = x.Agility + y.Agility
        Stamina     = x.Stamina + y.Stamina
        Intellect   = x.Intellect + y.Intellect
        Spirit      = x.Spirit + y.Spirit
        AttackPower = x.AttackPower + y.AttackPower
        Crit        = x.Crit + y.Crit
        Hit         = x.Hit + y.Hit
        Armor       = x.Armor + y.Armor
    }

let calculateEap statWeights (stats : ItemStats) =
    (stats.Strength |> float) * statWeights.Strength +
    (stats.Agility |> float) * statWeights.Agility +
    (stats.AttackPower |> float) +
    (stats.Crit |> float) * statWeights.Crit

let calculateSetValues statWeights set =
    let emptyStats = {
        Strength    = 0
        Agility     = 0
        Stamina     = 0
        Intellect   = 0
        Spirit      = 0
        AttackPower = 0
        Crit        = 0
        Hit         = 0
        Armor       = 0
    }

    let stats = List.fold (fun acc item -> addStats acc item.Stats) emptyStats set

    {
        Items = set
        Hit   = stats.Hit
        EAP   = calculateEap statWeights stats
    }

let filterSetsByHit hitThreshold sets =
    sets
    |> List.filter (fun set ->
        set
        |> List.fold (fun acc item -> 
            acc + item.Stats.Hit) 0 >= hitThreshold)

let filterSetsByArmorClass armorThreshold (sets : Item list list) =
    sets
    |> List.filter (
        List.fold (fun acc item ->
            acc && (item.ArmorClass.IsNone || item.ArmorClass.Value >= armorThreshold)) true)

let calculateBestSets number scenario sets =
    sets
    |> filterSetsByHit scenario.HitRequirement
    |> filterSetsByArmorClass scenario.ArmorRequirement
    |> List.map (calculateSetValues scenario.StatWeights)
    |> List.sortByDescending (fun set -> set.EAP)
    |> List.take number
module StatCalculator

open System
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

let sumStats (statsList : ItemStats list) =
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

    List.fold (fun acc (stats : ItemStats) -> addStats acc stats) emptyStats statsList

let calculateEap hitThreshold statWeights (stats : ItemStats) =
    let excessHit = 
        stats.Hit - hitThreshold
        |> fun h -> Math.Max(h, 0)
        |> float

    (stats.Strength |> float) * statWeights.Strength +
    (stats.Agility |> float) * statWeights.Agility +
    (stats.AttackPower |> float) +
    (stats.Crit |> float) * statWeights.Crit +
    excessHit * statWeights.ExcessHit

let calculateSetValues hitThreshold statWeights (set : Item list) =
    let statsFromItems = 
        set
        |> List.map (fun s -> s.Stats)
        |> sumStats

    let statsFromBonuses =
        set
        |> List.filter (fun item -> item.SetBonus.IsSome)
        |> List.map (fun item -> item.SetBonus.Value)
        |> List.groupBy (fun sb -> sb.Name)
        |> List.map (fun (_, bonuses) ->
            bonuses
            |> List.head
            |> fun sb -> sb.Bonuses
            |> List.filter (fun bonus -> bonus.ItemsRequired <= bonuses.Length)
            |> List.map (fun bonus -> bonus.Stats)
            |> sumStats)
        |> sumStats

    let totalStats = addStats statsFromItems statsFromBonuses

    {
        Items = set
        Hit   = totalStats.Hit
        EAP   = calculateEap hitThreshold statWeights totalStats
    }
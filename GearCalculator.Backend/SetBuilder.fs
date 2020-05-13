module SetBuilder

open System
open Models
open StatCalculator

let itemsAllowedBySlot slot =  
    match slot with
    | Finger | Trinket -> 2
    | _                -> 1

let updateSets (newItems : Item list list) (existingSets : Item list list) =
    match existingSets with
    | [] -> newItems
    | _  -> List.collect (fun items -> List.map (fun set -> List.append set items) existingSets) newItems

let generateCombinations items number =
    let rec combine n l =
        match n, l with
        | 0, _ -> [[]]
        | _, [] -> []
        | k, (h :: t) -> List.map ((@) [h]) (combine (k - 1) t) @ combine k t

    combine number items
    |> List.distinct

let generateSets items =
    let itemMap =
        items
        |> List.groupBy (fun item -> item.Slot)
        |> List.map (fun (slot, items) -> slot, itemsAllowedBySlot slot |> generateCombinations items)
        |> Map

    [
        Head
        Neck
        Shoulders
        Back
        Chest
        Wrist
        Hands
        Waist
        Legs
        Feet
        Finger
        Trinket
        Ranged
    ]
    |> List.fold (fun acc slot -> updateSets (Map.find slot itemMap) acc) []

let filterSetsByHit hitThreshold sets =
    List.filter (fun set -> set.Hit >= hitThreshold) sets

let filterSetsByArmorClass armorThreshold sets =
    sets
    |> List.filter (fun set ->
        set.Items
        |> List.fold (fun acc item ->
            acc && (item.ArmorClass.IsNone || item.ArmorClass.Value >= armorThreshold)) true)
   
let keepRelevantItems scenario items numberToKeep =
    let hasBonus, noBonus = List.partition (fun item -> item.SetBonus.IsSome) items
    
    let cappedNumberToKeep = Math.Min(numberToKeep, noBonus.Length)

    let bestItems = 
        match noBonus with
        | [] -> []
        | _  ->
            noBonus
            |> List.sortByDescending (fun item -> calculateEap scenario.HitRequirement scenario.StatWeights item.Stats)
            |> List.take cappedNumberToKeep
        
    hasBonus @ bestItems

let removeStrictlyWorseItems scenario items =
    items
    |> List.groupBy (fun item -> item.Slot)
    |> List.collect (fun (slot, itemsBySlot) -> 
        itemsBySlot
        |> List.groupBy (fun item -> item.Stats.Hit)
        |> List.collect (fun (hitAmount, itemsByHit) -> itemsAllowedBySlot slot |> keepRelevantItems scenario itemsByHit))

let removeItemsBelowArmorThreshold armorThreshold items =
    List.filter (fun (item : Item) -> item.ArmorClass.IsNone || item.ArmorClass.Value >= armorThreshold) items

let calculateBestSets number scenario (items : Item list) =
    items
    |> removeItemsBelowArmorThreshold scenario.ArmorRequirement
    |> removeStrictlyWorseItems scenario
    |> generateSets // using a cache here for generated sets by an item list could be useful?
    |> List.map (calculateSetValues scenario.HitRequirement scenario.StatWeights) // already performing eap calc when removing worse items, can we avoid doing it twice?
    |> filterSetsByHit scenario.HitRequirement
    |> List.sortByDescending (fun set -> set.EAP)
    |> List.take number
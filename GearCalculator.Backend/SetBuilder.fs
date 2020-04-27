module SetBuilder

open Models

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
        |> List.map (fun (slot, items) ->             
            slot, 
            match slot with
            | Finger | Trinket -> 2
            | _                -> 1
            |> generateCombinations items)
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
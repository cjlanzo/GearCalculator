open System.IO

open Models
open Database
open SetBuilder
open StatCalculator

let connStr = "Data Source=localhost; Database=WoWClassicGear; Integrated Security=true;"

let readItemsFromFile filename =
    filename
    |> File.ReadLines
    |> Seq.map (fun s -> s.Replace("'", "''"))

let stringifyScenario name (sets : Set list) =
    let divider = [ "-----------------------------" ]
    
    let setsAsText =
        sets
        |> List.mapi (fun i set ->
            let header = sprintf "Rank %d - EAP = %.2f - Hit = %d" (i + 1) set.EAP set.Hit
        
            let items =
                set.Items
                |> List.map (fun item -> 
                    sprintf "%A - %s" item.Slot item.Name)
                
            [ header ] @ items @ divider)
        |> List.concat

    [ name ] @ divider @ setsAsText

[<EntryPoint>]
let main args =

    use conn = createConnection connStr

    let items =
        readItemsFromFile "./mygear.txt" 
        |> Seq.map (queryItem conn)
        |> Seq.toList

    let sets = 
        items
        |> generateSets

    Presets.scenarios
    |> List.map (fun scenario -> scenario.Name, calculateBestSets 2 scenario sets)
    |> List.collect (fun (name, sets) -> stringifyScenario name sets)
    |> List.toArray
    |> fun lines -> File.WriteAllLines ("output.txt", lines)

    0


// TODO
// Add in set bonuses - verify that it works
// Add to source control - add release notes, etc
// Use dynamic programming to cache results and speed up run time
// Add ability for wish-listing gear
// Add in command line arguments
// Remove strictly worse items
// Make number of sets a CLA
// Better error handling when you don't get at least N sets when filtering by hit/armor class
// Write test cases
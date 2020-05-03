open System.IO

open Models
open Database
open SetBuilder

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
    
    Presets.standardScenarios
    //[ Presets.zgLeatherBoss; Presets.zgLeatherTrash; Presets.zgLeatherBossDw; Presets.zgLeatherBossDwHigh ]
    |> List.map (fun scenario -> scenario.Name, calculateBestSets 1 scenario items)
    |> List.collect (fun (name, sets) -> stringifyScenario name sets)
    |> List.toArray
    |> fun lines -> File.WriteAllLines ("test_output.txt", lines)

    0


// TODO
// Clean up needing so many text files
// Clean up presets
// Add FR scenario
// Add in command line arguments

// Look at how item rack stores sets and potentially write something to convert a set from this into an item rack set
// Make number of sets a CLA
// Build addon for exporting gear from character, bags, bank?
// Use dynamic programming to cache results and speed up run time
// Better error handling when you don't get at least N sets when filtering by hit/armor class
// Write test cases
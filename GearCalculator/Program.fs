open System.IO

open CommandLineSetup
open Options
open OptionMappers
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
    match parseArgs args with
    | ClaHelp        -> 0
    | ClaError err   -> failwithf "%s" err
    | ClaSuccess cla ->
        let options = mapClaToRunOptions cla

        use conn = createConnection connStr

        let items = 
            match options.InputOptions with
            | Text textOptions -> 
                readItemsFromFile textOptions.InputFile
                |> Seq.map (queryItem conn)
                |> Seq.toList
                
            | Database dbOptions -> 
                queryItems conn 
                |> List.filter (fun item -> item.Phase <= dbOptions.PhaseThreshold)
                |> List.filter (fun item -> not <| List.contains item.Name dbOptions.Exclusions)

        options.RunModeOptions
        |> mapRunOptionsToScenarios
        |> List.map (fun scenario -> scenario.Name, calculateBestSets options.SetsToInclude scenario items)
        |> List.collect (fun (name, sets) -> stringifyScenario name sets)
        |> List.toArray
        |> fun lines -> File.WriteAllLines (options.OutputFile, lines)

        0


// TODO
// Add rank 10 gear
// Add FR scenario

// Look at how item rack stores sets and potentially write something to convert a set from this into an item rack set
// Build addon for exporting gear from character, bags, bank?
// Use dynamic programming to cache results and speed up run time
// Better error handling when you don't get at least N sets when filtering by hit/armor class
// Write test cases
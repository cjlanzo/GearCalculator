module OptionMappers

open System
open CommandLineSetup
open Options
open Models
open Mappers

let parseStrToStrArr (str : string) =
    str.Split ","
    |> Array.map (fun s -> s.Trim())
    |> Array.toList

let parseStrToIntArr (str : string) =
    str |> parseStrToStrArr |> List.map int

let stringifyList (strList : string list) =
    String.concat ", " strList

let validateListSize size (li : 'a list) =
    match li.Length >= size with
    | true  -> li
    | false -> failwithf "List must contain at least %d elements" size

let zgOrStd statWeights = 
    match statWeights.Strength = 2. with
    | true  -> "Std"
    | false -> "ZG"

let mapClaToDatabaseInputOptions (cla : CommandLineArgs) =
    { 
        PhaseThreshold = cla.PhaseThreshold
        Exclusions     = parseStrToStrArr cla.ItemExclusions
    } |> Database

let mapClaToTextInputOptions (cla : CommandLineArgs) =
    match String.IsNullOrWhiteSpace cla.InputFile with
    | false -> Text { InputFile = cla.InputFile }
    | true  -> failwithf "InputFile cannot be blank in Text Mode"

let createStatWeights cla multiplier = 
    {
        Strength = cla.StrengthWeight * multiplier
        Agility = cla.AgilityWeight * multiplier
        Stamina = cla.StaminaWeight * multiplier
        Intellect = cla.IntellectWeight * multiplier
        Spirit = cla.SpiritWeight * multiplier
        Crit = cla.CritWeight
        ExcessHit = cla.ExcessHitWeight
    }

let mapClaToComparisonModeOptions (cla : CommandLineArgs) =
    let multiplier = 
        match cla.IncludeZgBuff with
        | true  -> 1.15
        | false -> 1.

    {
        SetsMustContain = 
            match String.IsNullOrWhiteSpace cla.SetsMustContain with
            | false -> parseStrToStrArr cla.SetsMustContain
            | true  -> failwithf "SetsMustContain cannot be blank in Comparison Mode"

        HitThreshold = 
            cla.HitThresholds
            |> parseStrToIntArr
            |> function
                | []     -> failwithf "HitThresholds must contain exactly 1 value but was blank"
                | [ ht ] -> ht
                | _      -> failwithf "HitThresholds must contain exactly 1 value but had more than 1"

        ArmorThreshold = 
            cla.ArmorThresholds
            |> parseStrToStrArr
            |> function
                | []     -> failwithf "ArmorThresholds must contain exactly 1 value but was blank"
                | [ at ] -> at
                | _      -> failwithf "ArmorThresholds must contain exactly 1 value but had more than 1"
            |> mapStrToArmorClassOption
            |> function
                | Some ac -> ac
                | None    -> failwithf "Could not parse %s to ArmorClass" cla.ArmorThresholds

        StatWeights = createStatWeights cla multiplier
    } |> ComparisonMode

let mapClaToScenarioModeOptions (cla : CommandLineArgs) =
    let statWeights =
        match cla.ExcludeStdStats with
        | true  -> []
        | false -> [ createStatWeights cla 1.0 ]
    
    let zgStatWeights =
        match cla.IncludeZgBuff with
        | false -> []
        | true  -> [ createStatWeights cla 1.15 ]

    {
        HitThresholds = cla.HitThresholds |> parseStrToIntArr |> validateListSize 1

        ArmorThresholds = 
            cla.ArmorThresholds
            |> parseStrToStrArr
            |> List.map mapStrToArmorClassOption
            |> List.map (fun opt ->
                match opt with
                | Some ac -> ac
                | None    -> failwithf "Could not parse %s to ArmorClass" cla.ArmorThresholds)
            |> validateListSize 1

        StatWeights = statWeights @ zgStatWeights
    } |> ScenarioMode


let mapClaToRunOptions (cla : CommandLineArgs) = 
    let inputOptions = 
        match cla.EnableDatabaseMode with
        | true  -> mapClaToDatabaseInputOptions cla
        | false -> mapClaToTextInputOptions cla

    let runModeOptions = 
        match cla.EnableComparisonMode with
        | true  -> mapClaToComparisonModeOptions cla
        | false -> mapClaToScenarioModeOptions cla
            
    {
        OutputFile     = cla.OutputFile
        SetsToInclude  = cla.SetsToInclude
        InputOptions   = inputOptions
        RunModeOptions = runModeOptions
    }

let mapScenarioModeOptionsToScenarios (options : ScenarioModeOptions) =
    options.StatWeights
    |> List.collect (fun sw ->
        options.HitThresholds
        |> List.collect (fun ht ->
            options.ArmorThresholds
            |> List.map (fun at ->
                {
                    Name             = sprintf "%s - %d Hit - %A+ - Excess Hit = %.1fAP" (zgOrStd sw) ht at sw.ExcessHit
                    HitRequirement   = ht
                    StatWeights      = sw
                    ArmorRequirement = at
                    RequiredItems    = []
                })))

let mapComparisonModeOptionsToScenarios (options : ComparisonModeOptions) =
    let bestSet =
        {
            HitRequirement   = options.HitThreshold
            StatWeights      = options.StatWeights
            ArmorRequirement = options.ArmorThreshold
            RequiredItems    = []
            Name             = 
                sprintf "%s - %d Hit - %A+ - Excess Hit = %.1fAP - Best possible set" 
                    (zgOrStd options.StatWeights) options.HitThreshold options.ArmorThreshold options.StatWeights.ExcessHit
        }

    let bestSetWithItem =
        {
            HitRequirement   = options.HitThreshold
            StatWeights      = options.StatWeights
            ArmorRequirement = options.ArmorThreshold
            RequiredItems    = options.SetsMustContain
            Name             = 
                sprintf "%s - %d Hit - %A+ - Excess Hit = %.1fAP - Best possible set including [%s]" 
                    (zgOrStd options.StatWeights) options.HitThreshold options.ArmorThreshold options.StatWeights.ExcessHit (stringifyList options.SetsMustContain)
        }
        
    [ bestSet ] @ [ bestSetWithItem ]

let mapRunOptionsToScenarios runOptions =
    match runOptions with
    | ScenarioMode smo   -> mapScenarioModeOptionsToScenarios smo
    | ComparisonMode cmo -> mapComparisonModeOptionsToScenarios cmo
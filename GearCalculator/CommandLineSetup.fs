module CommandLineSetup

open System
open System.Collections.Generic
open Fclp

type ClaParseResult<'T> =
    | ClaHelp 
    | ClaError of string
    | ClaSuccess of 'T

[<CLIMutable>]
type CommandLineArgs =
    {
        OutputFile           : string
        SetsToInclude        : int

        EnableDatabaseMode   : bool
        InputFile            : string
        PhaseThreshold       : int
        ItemExclusions       : string

        EnableComparisonMode : bool
        SetsMustContain      : string

        IncludeZgBuff        : bool
        HitThresholds        : string
        ArmorThresholds      : string

        StrengthWeight       : float
        AgilityWeight        : float
        StaminaWeight        : float
        IntellectWeight      : float
        SpiritWeight         : float
        CritWeight           : float
        ExcessHitWeight      : float
    }

let setupParser () =
    let parser = FluentCommandLineParser<CommandLineArgs>(IsCaseSensitive = false)
    parser.Setup(fun x -> x.OutputFile)          .As("outputFile")          .Required                 |> ignore
    parser.Setup(fun x -> x.SetsToInclude)       .As("setsToInclude")       .SetDefault(1)            |> ignore
    parser.Setup(fun x -> x.EnableDatabaseMode)  .As("enableDatabaseMode")  .SetDefault(false)        |> ignore
    parser.Setup(fun x -> x.InputFile)           .As("inputFile")           .SetDefault(String.Empty) |> ignore
    parser.Setup(fun x -> x.PhaseThreshold)      .As("phaseThreshold")      .SetDefault(6)            |> ignore
    parser.Setup(fun x -> x.ItemExclusions)      .As("itemExclusions")      .SetDefault(String.Empty) |> ignore
    parser.Setup(fun x -> x.EnableComparisonMode).As("enableComparisonMode").SetDefault(false)        |> ignore
    parser.Setup(fun x -> x.SetsMustContain)     .As("setsMustContain")     .SetDefault(String.Empty) |> ignore
    parser.Setup(fun x -> x.IncludeZgBuff)       .As("includeZgBuff")       .SetDefault(false)        |> ignore
    parser.Setup(fun x -> x.HitThresholds)       .As("hitThresholds")       .SetDefault(String.Empty) |> ignore
    parser.Setup(fun x -> x.ArmorThresholds)     .As("armorThresholds")     .SetDefault(String.Empty) |> ignore
    parser.Setup(fun x -> x.StrengthWeight)      .As("strengthWeight")      .SetDefault(2.)           |> ignore
    parser.Setup(fun x -> x.AgilityWeight)       .As("agilityWeight")       .SetDefault(1.5)          |> ignore
    parser.Setup(fun x -> x.StaminaWeight)       .As("staminaWeight")       .SetDefault(0.)           |> ignore
    parser.Setup(fun x -> x.IntellectWeight)     .As("intellectWeight")     .SetDefault(0.)           |> ignore
    parser.Setup(fun x -> x.SpiritWeight)        .As("spiritWeight")        .SetDefault(0.)           |> ignore
    parser.Setup(fun x -> x.CritWeight)          .As("critWeight")          .SetDefault(30.)          |> ignore
    parser.Setup(fun x -> x.ExcessHitWeight)     .As("excessHitWeight")     .SetDefault(0.)           |> ignore
    parser.SetupHelp("?") |> ignore
    
    parser

let printUsage (parserOptions : IEnumerable<Fclp.Internals.ICommandLineOption>) = 
    
    let maxNameLength = parserOptions |> Seq.map (fun clo -> clo.LongName.Length) |> Seq.max
    
    stdout.WriteLine()
    stdout.WriteLine "Arguments"
    stdout.WriteLine "---------"

    parserOptions |> Seq.iter (fun o -> stdout.WriteLine (sprintf "%*s - %s" maxNameLength o.LongName o.Description))

let parseArgs args =
    let parser = setupParser()
    let parseResults = parser.Parse args

    if parseResults.HelpCalled then
        printUsage parser.Options
        ClaHelp
    elif parseResults.HasErrors then
        printUsage parser.Options
        ClaError <| parseResults.ErrorText
    else
        ClaSuccess parser.Object
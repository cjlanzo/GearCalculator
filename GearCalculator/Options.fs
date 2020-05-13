module Options

open Models

type TextInputOptions = {
    InputFile : string
}

type DatabaseInputOptions = {
    PhaseThreshold : int
    Exclusions     : string list
}

type InputOptions = 
    | Text     of TextInputOptions
    | Database of DatabaseInputOptions

type ScenarioModeOptions = {
    HitThresholds   : int list
    ArmorThresholds : ArmorClass list
    StatWeights     : StatWeights list
}

type ComparisonModeOptions = {
    SetsMustContain : string list
    HitThreshold    : int
    ArmorThreshold  : ArmorClass
    StatWeights     : StatWeights
}

type RunModeOptions = 
    | ScenarioMode   of ScenarioModeOptions
    | ComparisonMode of ComparisonModeOptions

type RunOptions = {
    OutputFile     : string
    SetsToInclude  : int
    InputOptions   : InputOptions
    RunModeOptions : RunModeOptions
}
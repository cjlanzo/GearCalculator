module Presets

open Models

let standardStatWeights = 
    {
        Strength  = 2.
        Agility   = 1.5
        Stamina   = 0.
        Intellect = 0.
        Spirit    = 0.
        Crit      = 30.
        ExcessHit = 0.
    }

let zgStatWeights = 
    { standardStatWeights with
        Strength  = standardStatWeights.Strength * 1.15
        Agility   = standardStatWeights.Agility * 1.15
        Stamina   = standardStatWeights.Stamina * 1.15
        Intellect = standardStatWeights.Intellect * 1.15
        Spirit    = standardStatWeights.Spirit * 1.15
    }

let standardDualWieldStatWeights = 
    { standardStatWeights with
        ExcessHit = 10.
    }

let zgDualWieldStatWeights =
    { zgStatWeights with
        ExcessHit = 10.
    }

let zgDualWieldStatWeightsHigh =
    { zgStatWeights with
        ExcessHit = 18.
    }

let zgLeatherBoss =
    {
        Name = "ZG - 2H - 9 Hit - Leather+"
        HitRequirement = 9
        StatWeights = zgStatWeights
        ArmorRequirement = ArmorClass.Leather
    }
    
let zgMailBoss =
    {
        Name = "ZG - 2H - 9 Hit - Mail+"
        HitRequirement = 9
        StatWeights = zgStatWeights
        ArmorRequirement = ArmorClass.Mail
    }
        
let zgLeatherTrash =
    {
        Name = "ZG - 2H - 6 Hit - Leather+"
        HitRequirement = 6
        StatWeights = zgStatWeights
        ArmorRequirement = ArmorClass.Leather
    }
            
let zgMailTrash =
    {
        Name = "ZG - 2H - 6 Hit - Mail+"
        HitRequirement = 6
        StatWeights = zgStatWeights
        ArmorRequirement = ArmorClass.Mail
    }
                
let stdLeatherBoss =
    {
        Name = "Std - 2H - 9 Hit - Leather+"
        HitRequirement = 9
        StatWeights = standardStatWeights
        ArmorRequirement = ArmorClass.Leather
    }
                    
let stdMailBoss =
    {
        Name = "Std - 2H - 9 Hit - Mail+"
        HitRequirement = 9
        StatWeights = standardStatWeights
        ArmorRequirement = ArmorClass.Mail
    }
                        
let stdLeatherTrash =
    {
        Name = "Std - 2H - 6 Hit - Leather+"
        HitRequirement = 6
        StatWeights = standardStatWeights
        ArmorRequirement = ArmorClass.Leather
    }

let stdMailTrash =
    {
        Name = "Std - 2H - 6 Hit - Mail+"
        HitRequirement = 6
        StatWeights = standardStatWeights
        ArmorRequirement = ArmorClass.Mail
    }

let stdLeatherBossDw =
    {
        Name = "Std - DW - 6 hit - Leather+"
        HitRequirement = 6
        StatWeights = standardDualWieldStatWeights
        ArmorRequirement = ArmorClass.Leather
    }

let zgLeatherBossDw =
    {
        Name = "ZG - DW - 6 hit - Excess = 10AP - Leather+"
        HitRequirement = 6
        StatWeights = zgDualWieldStatWeights
        ArmorRequirement = ArmorClass.Leather
    }

let zgLeatherBossDwHigh =
    {
        Name = "ZG - DW - 6 hit - Excess = 18AP - Leather+"
        HitRequirement = 6
        StatWeights = zgDualWieldStatWeightsHigh
        ArmorRequirement = ArmorClass.Leather
    }

let standardScenarios =
    [
        zgLeatherBoss
        zgMailBoss
        zgLeatherTrash
        zgMailTrash
        stdLeatherBoss
        stdMailBoss
        stdLeatherTrash
        stdMailTrash
    ]

let stdVsDwScenario =
    [
        zgLeatherBoss
        zgLeatherBossDw
        stdLeatherBoss
        stdLeatherBossDw
    ]
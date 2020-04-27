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
    }

let zgStatWeights = 
    { standardStatWeights with
        Strength  = standardStatWeights.Strength * 1.15
        Agility   = standardStatWeights.Agility * 1.15
        Stamina   = standardStatWeights.Stamina * 1.15
        Intellect = standardStatWeights.Intellect * 1.15
        Spirit    = standardStatWeights.Spirit * 1.15
    }

let zgLeatherBoss =
    {
        Name = "ZG - 9 Hit - Leather+"
        HitRequirement = 9
        StatWeights = zgStatWeights
        ArmorRequirement = ArmorClass.Leather
    }
    
let zgMailBoss =
    {
        Name = "ZG - 9 Hit - Mail+"
        HitRequirement = 9
        StatWeights = zgStatWeights
        ArmorRequirement = ArmorClass.Mail
    }
        
let zgLeatherTrash =
    {
        Name = "ZG - 6 Hit - Leather+"
        HitRequirement = 6
        StatWeights = zgStatWeights
        ArmorRequirement = ArmorClass.Leather
    }
            
let zgMailTrash =
    {
        Name = "ZG - 6 Hit - Mail+"
        HitRequirement = 6
        StatWeights = zgStatWeights
        ArmorRequirement = ArmorClass.Mail
    }
                
let stdLeatherBoss =
    {
        Name = "Std - 9 Hit - Leather+"
        HitRequirement = 9
        StatWeights = standardStatWeights
        ArmorRequirement = ArmorClass.Leather
    }
                    
let stdMailBoss =
    {
        Name = "Std - 9 Hit - Mail+"
        HitRequirement = 9
        StatWeights = standardStatWeights
        ArmorRequirement = ArmorClass.Mail
    }
                        
let stdLeatherTrash =
    {
        Name = "Std - 6 Hit - Leather+"
        HitRequirement = 6
        StatWeights = standardStatWeights
        ArmorRequirement = ArmorClass.Leather
    }

let stdMailTrash =
    {
        Name = "Std - 6 Hit - Mail+"
        HitRequirement = 6
        StatWeights = standardStatWeights
        ArmorRequirement = ArmorClass.Mail
    }

let scenarios =
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
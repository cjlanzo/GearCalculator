module Models

type ItemSlot =
    | Head
    | Neck
    | Shoulders
    | Back
    | Chest
    | Wrist
    | Hands
    | Waist
    | Legs
    | Feet
    | Finger
    | Trinket
    | Ranged

type ArmorClass = 
    | Cloth = 1
    | Leather = 2
    | Mail = 3
    | Plate = 4

type ItemStats = {
    Strength    : int
    Agility     : int
    Stamina     : int
    Intellect   : int
    Spirit      : int
    AttackPower : int
    Crit        : int
    Hit         : int
    Armor       : int
}

type Bonus = {
    ItemsRequired : int
    Stats         : ItemStats
}

type SetBonus = {
    Name    : string
    Bonuses : Bonus list
}

type Item = {
    Name       : string
    Slot       : ItemSlot
    ArmorClass : ArmorClass option
    Stats      : ItemStats
    SetBonus   : SetBonus option
}

type DbItem = {
    Name        : string
    SlotName    : string
    ArmorClass  : string
    Strength    : int
    Agility     : int
    Stamina     : int
    Intellect   : int
    Spirit      : int
    AttackPower : int
    Crit        : int
    Hit         : int
    Armor       : int
    BonusName   : string
    Bonus       : string
}

type StatWeights = {
    Strength  : float
    Agility   : float
    Stamina   : float
    Intellect : float
    Spirit    : float
    Crit      : float
    ExcessHit : float
}

type Scenario = {
    Name             : string
    HitRequirement   : int
    StatWeights      : StatWeights
    ArmorRequirement : ArmorClass
}

type Set = {
    Items : Item list
    Hit   : int
    EAP   : float
}
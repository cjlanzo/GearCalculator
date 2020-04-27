module Mappers

open Models

let mapStrToArmorClassOption str =
    match str with
    | "Cloth"   -> Some ArmorClass.Cloth
    | "Leather" -> Some ArmorClass.Leather
    | "Mail"    -> Some ArmorClass.Mail
    | "Plate"   -> Some ArmorClass.Plate
    | _         -> None

let mapStrToItemSlot str =
    match str with
    | "Head"      -> Head
    | "Neck"      -> Neck
    | "Shoulders" -> Shoulders
    | "Back"      -> Back
    | "Chest"     -> Chest
    | "Wrist"     -> Wrist
    | "Hands"     -> Hands
    | "Waist"     -> Waist
    | "Legs"      -> Legs
    | "Feet"      -> Feet
    | "Finger"    -> Finger
    | "Trinket"   -> Trinket
    | "Ranged"    -> Ranged
    | _           -> failwithf "Could not map %s to a valid slot" str

let mapDbItemToItem (dbItem : DbItem) =
    {
        Name       = dbItem.Name
        Slot       = mapStrToItemSlot dbItem.SlotName
        ArmorClass = mapStrToArmorClassOption dbItem.ArmorClass

        Stats = {
            Strength    = dbItem.Strength
            Agility     = dbItem.Agility
            Stamina     = dbItem.Stamina
            Intellect   = dbItem.Intellect
            Spirit      = dbItem.Spirit
            AttackPower = dbItem.AttackPower
            Crit        = dbItem.Crit
            Hit         = dbItem.Hit
            Armor       = dbItem.Armor
        }
    }
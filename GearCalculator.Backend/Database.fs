module Database

open System.Data.SqlClient
open Dapper
open Models
open Mappers

let createConnection connStr =
    new SqlConnection(connStr)

let querySingle<'T> (conn : SqlConnection) (query : string) =
    conn.Query<'T> query
    |> Seq.head

let queryList<'T> (conn : SqlConnection) (query : string) =
    conn.Query<'T> query

let queryItems conn = 
    let query =
        "SELECT
            i.Name,
            s.SlotName,
            a.Name as ArmorClass,
            t.Strength,
            t.Agility,
            t.Stamina,
            t.Intellect,
            t.Spirit,
            t.AttackPower,
            t.Crit,
            t.Hit,
            t.Armor,
            b.Name as BonusName,
            b.Bonus,
            i.Phase
         FROM dbo.Items i
         JOIN dbo.ItemSlots s ON s.SlotID = i.ItemSlot
         JOIN dbo.ArmorClasses a ON a.ClassID = i.ArmorClass
         JOIN dbo.ItemStats t ON t.ItemID = i.ID
         LEFT JOIN dbo.SetBonuses b ON b.ID = i.SetBonus"
    
    queryList<DbItem> conn query
    |> Seq.map mapDbItemToItem
    |> Seq.toList

let queryItem conn itemName =
    let query =
        sprintf
            "SELECT
                i.Name,
                s.SlotName,
                a.Name as ArmorClass,
                t.Strength,
                t.Agility,
                t.Stamina,
                t.Intellect,
                t.Spirit,
                t.AttackPower,
                t.Crit,
                t.Hit,
                t.Armor,
                b.Name as BonusName,
                b.Bonus,
                i.Phase
             FROM dbo.Items i
             JOIN dbo.ItemSlots s ON s.SlotID = i.ItemSlot
             JOIN dbo.ArmorClasses a ON a.ClassID = i.ArmorClass
             JOIN dbo.ItemStats t ON t.ItemID = i.ID
             LEFT JOIN dbo.SetBonuses b ON b.ID = i.SetBonus
             WHERE i.Name = '%s'"
             itemName
    
    querySingle<DbItem> conn query
    |> mapDbItemToItem

let queryItemsByUser conn username =
    let query =
        sprintf
            "SELECT
                i.Name,
                s.SlotName,
                a.Name as ArmorClass,
                t.Strength,
                t.Agility,
                t.Stamina,
                t.Intellect,
                t.Spirit,
                t.AttackPower,
                t.Crit,
                t.Hit,
                t.Armor
             FROM dbo.UserSets x
             JOIN dbo.Users u ON u.ID = x.UserID
             JOIN dbo.Items i ON i.ID = x.ItemID
             JOIN dbo.ItemSlots s ON s.SlotID = i.ItemSlot
             JOIN dbo.ArmorClasses a ON a.ClassID = i.ArmorClass
             JOIN dbo.ItemStats t ON t.ItemID = i.ID
             WHERE u.Username = '%s'"
             username

    queryList<DbItem> conn query
    |> Seq.map mapDbItemToItem
    |> Seq.toList
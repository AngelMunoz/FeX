namespace FeX.Core

open Types

type Character =
    { Name: string
      Profession: Profession
      Skills: list<Skill>
      SkillPoints: int }

[<RequireQualifiedAccess>]
module Character =

    let Create (name: string) (profession: Profession) (skills: Option<list<Skill>>): Character =
        { Name = name
          SkillPoints = 0
          Profession = profession
          Skills =
              match skills with
              | Some skills -> skills
              | None -> List.empty<Skill> }

    let UpdateName (character: Character) (name: string) = { character with Name = name }

    let UpdateSkillPoints (character: Character) (skillPoints: int) =
        { character with
              SkillPoints = skillPoints }

    let PromoteProfession (character: Character): Result<Character, CharacterUpdateError> =
        let (family, stage) = character.Profession
        match stage with
        | Stage.First ->
            Ok
                { character with
                      Profession = (family, Stage.Second) }
        | Stage.Second ->
            Ok
                { character with
                      Profession = (family, Stage.Third) }
        | Stage.Third ->
            Error
                { Message = UpdateErrorMsg.AlreadyThirdStage
                  Property = CharacterProperty.Profession }

    let DemoteProfession (character: Character): Result<Character, CharacterUpdateError> =
        let (family, stage) = character.Profession
        match stage with
        | Stage.Third ->
            Ok
                { character with
                      Profession = (family, Stage.Second) }
        | Stage.Second ->
            Ok
                { character with
                      Profession = (family, Stage.First) }
        | Stage.First ->
            Error
                { Message = UpdateErrorMsg.AlreadyFirstStage
                  Property = CharacterProperty.Profession }

    let AddSkill (character: Character) (skill: Skill): Result<Character, CharacterUpdateError> =
        if skill.Points > character.SkillPoints then
            Error
                { Message = UpdateErrorMsg.NotEnoughSkillPoints
                  Property = CharacterProperty.SkillPoints }
        else
            match character.Skills
                  |> List.tryFind (fun s -> s = skill) with
            | Some _ ->
                Error
                    { Message = UpdateErrorMsg.SkillAlreadyPresent
                      Property = CharacterProperty.Skills }
            | None ->
                let (family, stage) = character.Profession
                let (skillFamily, skillStage) = skill.Profession

                let skillMatchesStage =
                    match stage, skillStage with
                    | Stage.Third, _ -> true
                    | Stage.Second, Stage.Second
                    | Stage.Second, Stage.First -> true
                    | Stage.First, Stage.First -> true
                    | _ -> false

                match skillFamily = family, skillMatchesStage with
                | false, _ ->
                    Error
                        { Message = UpdateErrorMsg.NotSameFamily
                          Property = CharacterProperty.Profession }
                | _, false ->
                    Error
                        { Message = UpdateErrorMsg.SkillIsHigherState
                          Property = CharacterProperty.Profession }
                | true, true ->
                    Ok
                        { character with
                              SkillPoints = character.SkillPoints - skill.Points
                              Skills = skill :: character.Skills }

    let RemoveSkill (character: Character) (skill: Skill): Result<Character, CharacterUpdateError> =
        match character.Skills
              |> List.tryFind (fun s -> s = skill) with
        | Some skill ->
            Ok
                { character with
                      SkillPoints = character.SkillPoints + skill.Points
                      Skills =
                          character.Skills
                          |> List.filter (fun s -> s <> skill) }
        | None ->
            Error
                { Message = UpdateErrorMsg.SkillNotPresent
                  Property = CharacterProperty.Skills }

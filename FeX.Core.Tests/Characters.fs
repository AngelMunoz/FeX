namespace FeX.Core.Tests

module Characters =

    open FeX.Core
    open FeX.Core.Types
    open Expecto

    [<Tests>]
    let tests =
        testList "Character Tests"
            [ testCase "Can Create Character"
              <| fun _ ->
                  let actual =
                      Character.Create "Expected" (Profession(Family.Charm, Stage.First)) None

                  let expected =
                      { Name = "Expected"
                        SkillPoints = 0
                        Profession = Profession(Family.Charm, Stage.First)
                        Skills = List.empty }

                  Expect.equal actual expected "Item has not been created"

              testCase "Can Promote Profession"
              <| fun _ ->
                  let actual =
                      { Name = "Actual"
                        SkillPoints = 0
                        Profession = Profession(Family.Charm, Stage.First)
                        Skills = List.empty }

                  let result = Character.PromoteProfession actual
                  Expect.isOk result "Could not Promote 'Promotable' Stage"

              testCase "Should Not Promote Third Stage"
              <| fun _ ->
                  let actual =
                      { Name = "Actual"
                        SkillPoints = 0
                        Profession = Profession(Family.Charm, Stage.Third)
                        Skills = List.empty }

                  let result = Character.PromoteProfession actual
                  Expect.isError result "Somehow I did promote 'Un-Promotable' Stage"

              testCase "Can Demote Profession"
              <| fun _ ->
                  let actual =
                      { Name = "Actual"
                        SkillPoints = 0
                        Profession = Profession(Family.Charm, Stage.Third)
                        Skills = List.empty }

                  let result = Character.DemoteProfession actual
                  Expect.isOk result "Could not demote 'Demotable' Stage"

              testCase "Can Add Skill"
              <| fun _ ->
                  let actual =
                      { Name = "Actual"
                        SkillPoints = 5
                        Profession = Profession(Family.Charm, Stage.Third)
                        Skills = List.empty }

                  let skill : Skill = 
                      { Name = "Actual Skill"
                        Profession = Profession(Family.Charm, Stage.First)
                        SkillType = SkillType.Active
                        Effect = 
                            { EffectType = EffectType.Damage(Some 5.5, Target.SingleTarget)
                              EffectDuration = EffectDuration.Instant
                              EffectCooldown = Some 1. }
                        Points = 5
                        Modifier = Modifier.Neutral
                      }

                  let result = Character.AddSkill actual skill
                  Expect.isOk result "Failed to Add a skill to the character"
                  match result with 
                  | Ok character ->
                      Expect.contains character.Skills skill "The Skill should be present in the 'Skills' property"
                  | Error _ -> ()

              testCase "Can Add Skill From Same or Lower Stage"
              <| fun _ -> 
                    let actual =
                        { Name = "Actual"
                          SkillPoints = 5
                          Profession = Profession(Family.Charm, Stage.Third)
                          Skills = List.empty }

                    let skill : Skill = 
                        { Name = "Actual Skill"
                          Profession = Profession(Family.Charm, Stage.First)
                          SkillType = SkillType.Active
                          Effect =
                            { EffectType = EffectType.Damage(Some 5.5, Target.SingleTarget)
                              EffectDuration = EffectDuration.Instant
                              EffectCooldown = Some 1. }
                          Points = 3
                          Modifier = Modifier.Neutral
                        }

                    let result = Character.AddSkill actual skill
                    Expect.isOk result "Failed to Add a skill to the character"
                    match result with 
                    | Ok character -> 
                        Expect.equal character.SkillPoints 2 "The amount of skill points should be 2"
                        Expect.contains character.Skills skill "The Skill should be present in the 'Skills' property"
                    | Error _ -> ()

              testCase "Should Not Add Skill With Higher Points"
              <| fun _ ->
                    let actual =
                        { Name = "Actual"
                          SkillPoints = 5
                          Profession = Profession(Family.Charm, Stage.First)
                          Skills = List.empty }

                    let skill : Skill = 
                        { Name = "Actual Skill"
                          Profession = Profession(Family.Charm, Stage.First)
                          SkillType = SkillType.Active
                          Effect =
                              { EffectType = EffectType.Damage(Some 5.5, Target.SingleTarget)
                                EffectDuration = EffectDuration.Instant
                                EffectCooldown = Some 1. }
                          Points = 10
                          Modifier = Modifier.Neutral
                        }

                    let result = Character.AddSkill actual skill
                    Expect.isError result "Should not have added the skill to the character"
                    match result with
                    | Error err ->
                        Expect.equal err.Message UpdateErrorMsg.NotEnoughSkillPoints "Error Message Should be 'NotEnoughSkillPoints'"
                    | Ok _ -> ()

              testCase "Should Not Add Skill With Different Family"
              <| fun _ ->
                    let actual =
                        { Name = "Actual"
                          SkillPoints = 5
                          Profession = Profession(Family.Charm, Stage.First)
                          Skills = List.empty }

                    let skill : Skill = 
                        { Name = "Actual Skill"
                          Profession = Profession(Family.Magic, Stage.First)
                          SkillType = SkillType.Active
                          Effect =
                              { EffectType = EffectType.Damage(Some 5.5, Target.SingleTarget)
                                EffectDuration = EffectDuration.Instant
                                EffectCooldown = Some 1. }
                          Points = 5
                          Modifier = Modifier.Neutral
                        }

                    let result = Character.AddSkill actual skill
                    Expect.isError result "Should not have added a magic skill to a charm character"
                    match result with
                    | Error err ->
                        Expect.equal err.Message UpdateErrorMsg.NotSameFamily "Error Message Should be 'NotSameFamily'"
                    | Ok _ -> ()

              testCase "Should Not Add Skill With Higher Stage"
              <| fun _ ->
                    let actual =
                        { Name = "Actual"
                          SkillPoints = 5
                          Profession = Profession(Family.Charm, Stage.First)
                          Skills = List.empty }

                    let skill : Skill = 
                        { Name = "Actual Skill"
                          Profession = Profession(Family.Charm, Stage.Second)
                          SkillType = SkillType.Active
                          Effect =
                              { EffectType = EffectType.Damage(Some 5.5, Target.SingleTarget)
                                EffectDuration = EffectDuration.Instant
                                EffectCooldown = Some 1. }
                          Points = 5
                          Modifier = Modifier.Neutral
                        }

                    let result = Character.AddSkill actual skill
                    Expect.isError result "Should not have added a Second Stage skill to a First Stage character"
                    match result with
                    | Error err ->
                        Expect.equal err.Message UpdateErrorMsg.SkillIsHigherState "Error Message Should be 'SkillIsHigherState'"
                    | Ok _ -> ()

              testCase "Can Remove Skill"
              <| fun _ ->
                    let skill : Skill = 
                        { Name = "Actual Skill"
                          Profession = Profession(Family.Charm, Stage.First)
                          SkillType = SkillType.Active
                          Effect =
                              { EffectType = EffectType.Damage(Some 5.5, Target.SingleTarget)
                                EffectDuration = EffectDuration.Instant
                                EffectCooldown = Some 1. }
                          Points = 5
                          Modifier = Modifier.Neutral
                        }
                    let actual =
                        { Name = "Actual"
                          SkillPoints = 5
                          Profession = Profession(Family.Charm, Stage.Third)
                          Skills = [skill] }


                    let result = Character.RemoveSkill actual skill
                    Expect.isOk result "Failed to Remove a skill of the character"
                    match result with 
                    | Ok character ->
                        Expect.isEmpty character.Skills "The Skill should be present in the 'Skills' property"
                    | Error _ -> ()

              testCase "Should not Remove non-existant Skill"
              <| fun _ ->
                    let skill : Skill = 
                        { Name = "Actual Skill"
                          Profession = Profession(Family.Charm, Stage.First)
                          SkillType = SkillType.Active
                          Effect =
                              { EffectType = EffectType.Damage(Some 5.5, Target.SingleTarget)
                                EffectDuration = EffectDuration.Instant
                                EffectCooldown = Some 1. }
                          Points = 5
                          Modifier = Modifier.Neutral
                        }
                    let actual =
                        { Name = "Actual"
                          SkillPoints = 5
                          Profession = Profession(Family.Charm, Stage.Third)
                          Skills = List.empty }


                    let result = Character.RemoveSkill actual skill
                    Expect.isError result "Somehow Did remove a skill that was not present in the character"
                    match result with 
                    | Error err ->
                        Expect.equal err.Message UpdateErrorMsg.SkillNotPresent "The Error Message should be 'SkillNotPresent'"
                    | Ok _ -> () 
            ]

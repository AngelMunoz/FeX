namespace FeX.Core

module Types =

    [<RequireQualifiedAccess>]
    type Family =
        | Strength
        | Magic
        | Charm
        | Sensory

    [<RequireQualifiedAccess>]
    type Stage =
        | First
        | Second
        | Third

    type Profession = Family * Stage

    [<AutoOpen>]
    type Modifier =
        | Fire
        | Earth
        | Water
        | Air
        | Light
        | Dark
        | Neutral

    type SkillType =
        | Passive
        | Active

    type EffectDuration =
        | Instant
        | Lapse of lapse: float
        | Loop of times: int * looplapse: float

    type Target =
        | NoTarget
        | SingleTarget
        | MultiTarget of targets: int
        | AoE of area: float

    type EffectType =
        | Damage of Amount: Option<float> * Target: Target
        | Heal of Amount: Option<float> * Target: Target
        | Replenish of Amount: Option<float> * Target: Target


    type Effect =
        { EffectType: EffectType
          EffectDuration: EffectDuration
          EffectCooldown: Option<float> }

    type Skill =
        { Name: string
          Profession: Profession
          SkillType: SkillType
          Effect: Effect
          Points: int
          Modifier: Modifier }

    [<RequireQualifiedAccess>]
    type CharacterProperty =
        | Name
        | Profession
        | Skills
        | SkillPoints

    [<RequireQualifiedAccess>]
    type UpdateErrorMsg =
        | AlreadyThirdStage
        | AlreadyFirstStage
        | NotEnoughSkillPoints
        | NotSameFamily
        | SkillIsHigherState
        | SkillNotPresent
        | SkillAlreadyPresent

    type CharacterUpdateError =
        { Message: UpdateErrorMsg
          Property: CharacterProperty }

    [<RequireQualifiedAccess>]
    type SceneType = 
        | LoadingScreen
        | Plaza
        | WildArea
        | WaitingRoom
        | BossRoom
        | SelectionRoom

    [<RequireQualifiedAccess>]
    type GameObjectKind = 
        | NPC
        | Enemy
        | Player
        | DestructibleObject
        | IndestructibleObject

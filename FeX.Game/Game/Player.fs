namespace FeX.Game

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open FeX.Core
open FeX.Core.Types


type Player =
    { spriteBatch: Option<SpriteBatch>
      texture: Option<Texture2D>
      speed: Option<float32>
      position: Option<Vector2>
      life: Option<int> }

[<RequireQualifiedAccess>]
module Player =

    let init (origin: Vector2) (speed: float32) (player: Player) =
        { player with
              position = Some origin
              speed = Some speed }

    let loadContent (batch: SpriteBatch) (texture: Texture2D) (player: Player) =
        { player with
              spriteBatch = Some batch
              texture = Some texture }

    let update (position: Vector2) (player: Player) = { player with position = Some position }

    let draw (rotation: float32) (preferredOrigin: Vector2) (player: Player) =
        match player.spriteBatch with
        | Some batch ->
            batch.Begin()
            batch.Draw
                (player.texture |> Option.defaultValue null,
                 player.position
                 |> Option.defaultValue preferredOrigin,
                 Nullable(),
                 Color.White,
                 rotation,
                 preferredOrigin,
                 1.f,
                 SpriteEffects.None,
                 0.f)
            batch.End()
        | None -> ()

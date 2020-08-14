namespace FeX.Game

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open MonoGame.Extended
open FeX.Core
open Types

type Player =
    { texture: Option<Texture2D>
      textureName: string
      speed: float32
      position: Vector2
      bounds: Option<Rectangle>
      playerIndex: PlayerIndex }

    static member Update (position: Vector2) (bounds: Option<Rectangle>) (player: Player) =
        { player with
              position = position
              bounds = bounds |> Option.orElse player.bounds }

    interface GameObject with

        member __.Kind: GameObjectKind = GameObjectKind.Player

        member this.Load(content: ContentManager) =
            let texture =
                Option.ofObj (content.Load<Texture2D>(this.textureName))

            let bounds =
                match texture with
                | Some texture -> Some texture.Bounds
                | None -> None

            { this with
                  texture = texture
                  bounds = bounds } :> GameObject

        member this.Update (gameTime: GameTime) (camera: Option<OrthographicCamera>): GameObject =
            let padstate = GamePad.GetState(this.playerIndex)
            let kstate = Keyboard.GetState()

            let elapsed =
                gameTime.ElapsedGameTime.TotalSeconds |> float32

            let finalPosition =
                if padstate.IsConnected then
                    /// TODO: change Vector2(1.f, -1.f) for X/Y Inverted control settings
                    let direction =
                        padstate.ThumbSticks.Left * Vector2(1.f, -1.f)
                    /// TODO: change 0.05f for a better suited value (controller sensitivity)
                    this.position
                    + direction
                    * ((this.speed * 0.05f) + elapsed)
                else
                    Position.moveWithArrows kstate this.speed elapsed this.position

            let bounds =
                let size =
                    match this.texture with
                    | Some texture -> texture.Bounds.Size
                    | None -> Point.Zero

                Rectangle(finalPosition.ToPoint(), size)

            match camera with
            | Some camera -> 
                let camerapos = camera.WorldToScreen(finalPosition)
                camera.Move(camerapos)
            | None -> ()
            Player.Update finalPosition (Some bounds) this :> GameObject

        member this.Draw (gameTime: GameTime) (batch: SpriteBatch): unit =
            match this.texture with
            | Some texture -> batch.Draw(texture, this.position, Color.White)
            | None -> ()

        member this.Dispose(): unit =
            match this.texture with
            | Some texture -> texture.Dispose()
            | None -> ()

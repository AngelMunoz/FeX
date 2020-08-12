namespace FeX.Game

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open FeX.Core
open Types

type Player =
    { texture: Texture2D
      speed: float32
      position: Vector2
      bounds: Rectangle
      playerIndex: PlayerIndex }

    static member Update (position: Vector2) (bounds: Option<Rectangle>) (player: Player) =
        { player 
            with 
                position = position;
                bounds = 
                    match bounds with 
                    | Some bounds -> bounds 
                    | None -> player.bounds }

    interface GameObject with
        member this.Load (content: ContentManager) =
            this :> GameObject

        member this.Update(gameTime: GameTime): GameObject =
            let padstate = GamePad.GetState(this.playerIndex)
            let kstate = Keyboard.GetState()

            let elapsed =
                gameTime.ElapsedGameTime.TotalSeconds |> float32

            let finalPosition = 
                if padstate.IsConnected then
                    /// TODO: change Vector2(1.f, -1.f) for X/Y Inverted control settings
                    let direction = padstate.ThumbSticks.Left * Vector2(1.f, -1.f)
                    /// TODO: change 0.05f for a better suited value (controller sensitivity)
                    this.position + direction * ((this.speed * 0.05f) + elapsed)
                else
                    Position.moveWithArrows kstate this.speed elapsed this.position

            let bounds = 
                Rectangle(finalPosition.ToPoint(), this.texture.Bounds.Size)

            Player.Update finalPosition (Some bounds) this
            :> GameObject

        member this.Draw(gameTime: GameTime) (batch: SpriteBatch): unit =
            batch.Draw(this.texture, this.position, Color.White)

        member this.Dispose(): unit = 
            this.texture.Dispose()
namespace FeX.Game

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open MonoGame.Extended
open FeX.Core.Types
open Types

type LoadingScene =
    { name: string
      artworkName: string
      batch: Option<SpriteBatch>
      artwork: Option<Texture2D> }

    interface Scene with
        member __.SceneType = SceneType.LoadingScreen

        member this.Initialize(_: OrthographicCamera): Scene =
            // Add Initialization code for the scene
            this :> Scene

        member this.LoadContent (spriteBatch: SpriteBatch) (content: ContentManager): Scene =
            // Load content, sprites whatsovever
            let artwork =
                Option.ofObj (content.Load<Texture2D>(this.artworkName))

            { this with
                  artwork = artwork
                  batch = Some spriteBatch } :> Scene

        member this.Update(gameTime: GameTime): Scene =
            // Update logic
            this :> Scene


        member this.Draw (gameTime: GameTime) (target: Option<Rectangle>): unit =
            // Draw stuff on screen
            match this.batch, this.artwork with
            | Some batch, Some artwork ->
                batch.Begin()
                if target.IsNone
                then batch.Draw(artwork, Vector2.Zero, Color.White)
                else batch.Draw(artwork, target.Value, Color.White)
                batch.End()
            | _ -> ()

        member this.Dispose(): unit =
            match this.artwork with
            | Some artwork -> artwork.Dispose()
            | None -> ()

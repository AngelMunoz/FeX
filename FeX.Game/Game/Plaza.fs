namespace FeX.Game

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open MonoGame.Extended
open FeX.Core.Types
open Types

type PlazaScene =
    { name: string
      batch: Option<SpriteBatch>
      mapStructure: array<byte>
      artworkName: string
      artwork: Option<Texture2D>
      decorations: array<Decoration>
      players: array<Player>
      camera: Option<OrthographicCamera> }

    interface Scene with
        member __.SceneType = SceneType.Plaza

        member this.Initialize(camera: OrthographicCamera): Scene =
            // Add Initialization code for the scene
            { this with camera = Some camera } :> Scene

        member this.LoadContent (spriteBatch: SpriteBatch) (content: ContentManager): Scene =
            // Load content, sprites whatsovever
            let artwork =
                Option.ofObj (content.Load<Texture2D>(this.artworkName))

            let decorations =
                this.decorations
                |> Array.map (fun d -> ((d :> GameObject).Load content) :?> Decoration)

            let players =
                this.players
                |> Array.map (fun p -> ((p :> GameObject).Load content) :?> Player)

            { this with
                  artwork = artwork
                  batch = Some spriteBatch
                  decorations = decorations
                  players = players } :> Scene

        member this.Update(gameTime: GameTime): Scene =
            // Update logic
            let decorations =
                this.decorations
                |> Array.map (fun d ->
                    let go: GameObject = (d :> GameObject)
                    let d: GameObject = go.Update gameTime None
                    d :?> Decoration)

            let players =
                this.players
                |> Array.map (fun p ->
                    match p.playerIndex with
                    | PlayerIndex.One ->
                        let go: GameObject = (p :> GameObject)
                        let p: GameObject = go.Update gameTime this.camera
                        p :?> Player
                    | _ -> ((p :> GameObject).Update gameTime None) :?> Player)

            { this with
                  decorations = decorations
                  players = players } :> Scene


        member this.Draw (gameTime: GameTime) (target: Option<Rectangle>): unit =
            // Draw stuff on screen
            match this.batch, this.artwork, this.camera with
            | Some batch, Some artwork, Some camera ->
                let matrix = camera.GetViewMatrix()
                batch.Begin(transformMatrix = Nullable(matrix))
                if target.IsNone
                then batch.Draw(artwork, Vector2.Zero, Color.White)
                else batch.Draw(artwork, target.Value, Color.White)
                this.decorations
                |> Array.iter (fun d -> (d :> GameObject).Draw gameTime batch)
                this.players
                |> Array.iter (fun p -> (p :> GameObject).Draw gameTime batch)
                batch.End()
            | _ -> ()

        member this.Dispose(): unit =
            match this.artwork with
            | Some artwork -> artwork.Dispose()
            | None -> ()

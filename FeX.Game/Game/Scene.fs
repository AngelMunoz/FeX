namespace FeX.Game

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Types
open Microsoft.Xna.Framework.Content

type Scene =
    { name: string
      spriteBatch: Option<SpriteBatch>
      gameObjects: list<GameObject>
    }

module Scene =

    let init (scene: Scene) : Scene =
        // Add Initialization code for the scene
        scene

    let load (spriteBatch: SpriteBatch) (content: ContentManager) (scene: Scene) : Scene =
        // Load content, sprites whatsovever
        let objects = 
            if scene.name = "Sample Scene" then 
                let player : Player =
                    let texture = content.Load<Texture2D>("ball")
                    { texture = texture
                      speed = 250.f
                      position = Vector2.Zero
                      playerIndex = PlayerIndex.One
                      bounds = Rectangle(Vector2.Zero.ToPoint(), texture.Bounds.Size) }
                [player :> GameObject]
            else 
                scene.gameObjects

        let gameObjects = 
             objects |> List.map (fun go -> go.Load content)
        { scene with spriteBatch = Some spriteBatch; gameObjects = gameObjects }

    let update (gametime: GameTime) (scene: Scene) : Scene = 
        // Update logic
        let gameObjects = scene.gameObjects |> List.map (fun go -> go.Update gametime)
        { scene with gameObjects = gameObjects }

    let draw (gametime: GameTime) (scene: Scene) : Scene =
        // Draw stuff on screen
        match scene.spriteBatch with 
        | Some batch ->
            batch.Begin()
            scene.gameObjects 
            |> List.iter (fun go -> go.Draw gametime batch)
            batch.End()
        | None -> ()
        scene





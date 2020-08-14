namespace FeX.Game

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open MonoGame.Extended
open FeX.Core
open Types


type Decoration =
    { textureName: string
      texture: Option<Texture2D>
      position: Option<Vector2>
      bounds: Option<Rectangle>
      passThrough: bool }

    interface GameObject with
        member __.Kind: GameObjectKind = GameObjectKind.IndestructibleObject

        member this.Load(content: ContentManager): GameObject =
            let texture =
                Option.ofObj (content.Load<Texture2D>(this.textureName))

            let bounds =
                match texture with
                | Some texture -> Some texture.Bounds
                | None -> None

            { this with
                  texture = texture
                  bounds = bounds } :> GameObject

        member this.Update (gameTime: GameTime) (_: Option<OrthographicCamera>): GameObject = this :> GameObject

        member this.Draw (gameTime: GameTime) (batch: SpriteBatch): unit =
            match this.texture with
            | Some texture -> batch.Draw(texture, texture.Bounds.Center.ToVector2(), Color.White)
            | None -> ()

        member this.Dispose(): unit =
            match this.texture with
            | Some texture -> texture.Dispose()
            | None -> ()

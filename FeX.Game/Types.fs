namespace FeX.Game

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
open MonoGame.Extended
open FeX.Core.Types

module Types =

    type GameObject =
        inherit IDisposable

        abstract Kind: GameObjectKind
        abstract Load: content:ContentManager -> GameObject
        abstract Update: gameTime:GameTime -> camera:Option<OrthographicCamera> -> GameObject
        abstract Draw: gameTime:GameTime -> batch:SpriteBatch -> unit


    type Scene =
        inherit IDisposable

        abstract SceneType: SceneType
        abstract Initialize: camera:OrthographicCamera -> Scene
        abstract LoadContent: batch:SpriteBatch -> content:ContentManager -> Scene
        abstract Update: gameTIme:GameTime -> Scene
        abstract Draw: gameTime:GameTime -> renderTarget:Option<Rectangle> -> unit

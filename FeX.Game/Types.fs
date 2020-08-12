namespace FeX.Game

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content

module Types =

    type GameObject =
        inherit IDisposable

        abstract Load : ContentManager -> GameObject
        abstract Update : GameTime -> GameObject
        abstract Draw : GameTime -> SpriteBatch -> unit


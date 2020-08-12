namespace FeX.Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

[<RequireQualifiedAccess>]
module Position =

    let moveWithArrows (kstate: KeyboardState) (speed: float32) (elapsed: float32) (position: Vector2) =
        match kstate.GetPressedKeys() with
        | keys when keys
                    |> Array.contains (Keys.Up)
                    && keys |> Array.contains (Keys.Left) ->
            let y = position.Y - (speed * elapsed)
            let x = position.X - (speed * elapsed)
            (x, y)
        | keys when keys
                    |> Array.contains (Keys.Up)
                    && keys |> Array.contains (Keys.Right) ->
            let y = position.Y - (speed * elapsed)
            let x = position.X + (speed * elapsed)
            (x, y)
        | keys when keys
                    |> Array.contains (Keys.Down)
                    && keys |> Array.contains (Keys.Left) ->
            let y = position.Y + (speed * elapsed)
            let x = position.X - (speed * elapsed)
            (x, y)
        | keys when keys
                    |> Array.contains (Keys.Down)
                    && keys |> Array.contains (Keys.Right) ->
            let y = position.Y + (speed * elapsed)
            let x = position.X + (speed * elapsed)
            (x, y)
        | keys when keys |> Array.contains (Keys.Up) ->
            let y = position.Y - (speed * elapsed)
            let x = position.X
            (x, y)
        | keys when keys |> Array.contains (Keys.Down) ->
            let y = position.Y + (speed * elapsed)
            let x = position.X
            (x, y)
        | keys when keys |> Array.contains (Keys.Right) ->
            let y = position.Y
            let x = position.X + (speed * elapsed)
            (x, y)
        | keys when keys |> Array.contains (Keys.Left) ->
            let y = position.Y
            let x = position.X - (speed * elapsed)
            (x, y)
        | _ -> (position.X, position.Y)
        |> Vector2

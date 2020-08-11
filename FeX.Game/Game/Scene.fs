namespace FeX.Game

open System
open Microsoft.Xna.Framework


type Scene =
    { name: string }


module Scene =

    let init () =
        // Add Initialization code for the scene
        raise (NotImplementedException "Scene init not implemented")

    let load () =
        // Load content, sprites whatsovever
        raise (NotImplementedException "Scene load not implemented")

    let update (gametime: GameTime) = 
        // Update logic
        raise (NotImplementedException "Scene update not implemented") 

    let draw (gametime: GameTime) =
        // Draw stuff on screen
        raise (NotImplementedException "Scene draw not implemented")    




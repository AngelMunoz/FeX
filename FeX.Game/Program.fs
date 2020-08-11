namespace FeX.Game

open System

module Main =

    [<STAThread>]
    [<EntryPoint>]
    let main argv =
        use game = new FeXGame()
        game.Run()
        0

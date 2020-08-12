namespace FeX.Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type FeXGame() as this =
    inherit Game()

    let scene: Scene = 
        { name = "Sample Scene"
          spriteBatch = None 
          gameObjects = [] }

    let mutable currentScene : Option<Scene> = 
        Some scene

    let mutable scenes = [scene]

    [<DefaultValue>]
    val mutable private graphics: GraphicsDeviceManager
    [<DefaultValue>]
    val mutable private spriteBatch : SpriteBatch

    do
        this.graphics <- new GraphicsDeviceManager(this)
        this.Content.RootDirectory <- "Content"
        this.IsMouseVisible <- true

        this.Window.AllowUserResizing <- true


    override __.Initialize() =
        // TODO: Add your initialization logic here
        match currentScene with
        | Some scene -> 
            currentScene <- Scene.init scene |> Some
        | None -> 
            currentScene <- List.tryHead scenes
        base.Initialize()

    override __.LoadContent() =
        // TODO: Load your content here
        this.spriteBatch <- new SpriteBatch(this.graphics.GraphicsDevice)
        match currentScene with
        | Some scene -> 
            currentScene <-
                scene
                |> Scene.load (this.spriteBatch) (this.Content)
                |> Some

        | None -> ()
        base.LoadContent()
            

    override __.Update(gameTime: GameTime) =
        if GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed
           || Keyboard.GetState().IsKeyDown(Keys.Escape) then
            this.Exit()

        // TODO: Add your update logic here
        match currentScene with 
        | Some scene ->
            currentScene <- 
                scene 
                |> Scene.update gameTime
                |> Some
        | None -> ()

        base.Update(gameTime)

    override __.Draw(gameTime: GameTime) =
        this.GraphicsDevice.Clear(Color.CornflowerBlue)

        // TODO: Add your drawing code here
        match currentScene with 
        | Some scene ->
            currentScene <-
                scene 
                |> Scene.draw gameTime
                |> Some
        | None -> ()

        base.Draw(gameTime)

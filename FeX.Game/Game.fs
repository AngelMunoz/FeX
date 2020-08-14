namespace FeX.Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open FeX.Core.Types
open Types
open System.Threading.Tasks
open Microsoft.Xna.Framework.Input.Touch
open MonoGame.Extended
open MonoGame.Extended.ViewportAdapters

type FeXGame() as this =
    inherit Game()

    let decoration1: Decoration =
        { textureName = "Decorations/SnowPile"
          texture = None
          position = Some(Vector2(200.f, -500.f))
          bounds = None
          passThrough = false }

    let decoration2: Decoration =
        { textureName = "Decorations/SnowPile"
          texture = None
          position = Some(Vector2(100.f, -400.f))
          bounds = None
          passThrough = false }

    let loadingScene: Scene =
        { name = "FirstLoadingScreen"
          artworkName = "LoadingScreens/Alone"
          batch = None
          artwork = None } :> Scene

    let player: Player =
        { textureName = "ball"
          texture = None
          speed = 200.f
          position = Vector2.Zero
          bounds = None
          playerIndex = PlayerIndex.One }

    let plazaScene: Scene =
        { name = "PlazaScreen"
          batch = None
          mapStructure = Array.empty<byte>
          artworkName = "Plazas/SampleMap"
          artwork = None
          camera = None
          decorations = [| decoration1; decoration2 |]
          players = [| player |] } :> Scene

    let mutable scenes: list<Scene> = [ loadingScene; plazaScene ]

    let mutable currentScene: Scene = scenes.Head

    let mutable isLoading: bool = false

    [<DefaultValue>]
    val mutable private graphics: GraphicsDeviceManager

    [<DefaultValue>]
    val mutable private spriteBatch: SpriteBatch

    [<DefaultValue>]
    val mutable private camera: OrthographicCamera

    do
        this.graphics <- new GraphicsDeviceManager(this)
        this.Content.RootDirectory <- "Content"
        this.IsMouseVisible <- true
        this.Window.AllowUserResizing <- true
        TouchPanel.EnableMouseTouchPoint <- true


    override __.Initialize() =
        // TODO: Add your initialization logic here
        isLoading <- true
        let viewportAdapter =
            let vp = this.graphics.GraphicsDevice.Viewport
            new BoxingViewportAdapter(this.Window, this.graphics.GraphicsDevice, vp.Width, vp.Height)

        this.camera <- OrthographicCamera(viewportAdapter)
        this.spriteBatch <- new SpriteBatch(this.graphics.GraphicsDevice)
        scenes <- scenes |> List.map(fun s -> s.Initialize this.camera)
        base.Initialize()

    override __.LoadContent() =
        // TODO: Load your content here
        scenes <-
            scenes
            |> List.map (fun scene -> scene.LoadContent this.spriteBatch this.Content)
        currentScene <- scenes.[scenes.Length - 1]
        async {
            do! Task.Delay 1500 |> Async.AwaitTask
            isLoading <- false
        }
        |> Async.StartImmediate
        base.LoadContent()


    override __.Update(gameTime: GameTime) =
        if GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed
           || Keyboard.GetState().IsKeyDown(Keys.Escape) then
            this.Exit()

        // TODO: Add your update logic here
        scenes <-
            scenes
            |> List.map (fun scene -> scene.Update gameTime)
        currentScene <- scenes.[scenes.Length - 1].Update gameTime

        base.Update(gameTime)

    override __.Draw(gameTime: GameTime) =
        this.GraphicsDevice.Clear(Color.SandyBrown)

        // TODO: Add your drawing code here
        let vp = this.graphics.GraphicsDevice.Viewport
        match isLoading with
        | true ->
            match scenes
                  |> List.tryFind (fun scene -> scene.SceneType = SceneType.LoadingScreen) with
            | Some scene ->
                let target = Rectangle(0, 0, vp.Width, vp.Height)
                scene.Draw gameTime (Some(target))
            | None ->
                match currentScene.SceneType with
                | SceneType.LoadingScreen ->
                    let target = Rectangle(0, 0, vp.Width, vp.Height)
                    currentScene.Draw gameTime (Some target)
                | _ -> currentScene.Draw gameTime None
        | false -> currentScene.Draw gameTime None

        base.Draw(gameTime)

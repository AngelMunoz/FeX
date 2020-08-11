namespace FeX.Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type FeXGame() as this =
    inherit Game()

    let defaultSpeed = 250.f
    let mutable preferredOrigin = Vector2.Zero

    let mutable player: Player =
        { spriteBatch = None
          texture = None
          speed = None
          position = None
          life = Some 100 }

    [<DefaultValue>]
    val mutable private graphics: GraphicsDeviceManager

    do
        this.graphics <- new GraphicsDeviceManager(this)
        this.Content.RootDirectory <- "Content"
        this.IsMouseVisible <- true

        this.Window.AllowUserResizing <- true


    override __.Initialize() =
        // TODO: Add your initialization logic here
        preferredOrigin <-
            Vector2
                (this.graphics.PreferredBackBufferWidth
                 |> float32
                 |> (/) 2.f,
                 this.graphics.PreferredBackBufferHeight
                 |> float32
                 |> (/) 2.f)

        player <- Player.init preferredOrigin defaultSpeed <| player

        base.Initialize()

    override __.LoadContent() =
        player <-
            Player.loadContent
            <| new SpriteBatch(this.GraphicsDevice)
            <| this.Content.Load<Texture2D>("ball")
            <| player

    override __.Update(gameTime: GameTime) =
        if GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed
           || Keyboard.GetState().IsKeyDown(Keys.Escape) then
            this.Exit()

        // TODO: Add your update logic here

        let kstate = Keyboard.GetState()
        let gstate = GamePad.GetState(PlayerIndex.One)

        let speed =
            player.speed |> Option.defaultValue defaultSpeed

        let elapsed =
            gameTime.ElapsedGameTime.TotalSeconds |> float32

        let thumbstickpos =
            gstate.ThumbSticks.Left * Vector2(1.f, -1.f)

        let playerpos =
            match player.position with
            | Some pos -> Some(thumbstickpos + pos)
            | None -> Some Vector2.Zero



        player <-
            player
            |> Player.update (Position.moveWithArrows kstate speed elapsed playerpos)

        base.Update(gameTime)

    override __.Draw(gameTime: GameTime) =
        this.GraphicsDevice.Clear(Color.CornflowerBlue)

        // TODO: Add your drawing code here
        this.graphics.GraphicsDevice.Clear(Color.Black)

        Player.draw <| 0.f <| preferredOrigin <| player

        base.Draw(gameTime)

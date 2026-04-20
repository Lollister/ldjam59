using Godot;
using System;

public partial class GameState : Node
{
    public enum State
    {
        MainMenu,
        SettingsMenu,
        CreditsMenu,
        GameRunning,
        GameLost,
    }

    [Export] public State CurrentState { get; set; }
    [Export] public Control MainMenu { get; set; }
    [Export] public Control IngameUI { get; set; }
    [Export] public TransitionCamera TransitionCamera { get; set; }
    [Export] public Camera3D MainMenuCamera { get; set; }
    [Export] public CameraInteraction IngameCamera { get; set; }
    [Export] public RoundManager RoundManager { get; set; }

    private Button btnNewGame;
    private Button btnSettings;
    private Button btnCredits;
    private Button btnQuit;

    public override void _Ready()
    {
        btnNewGame = MainMenu.FindChild("StartGame") as Button;
        btnSettings = MainMenu.FindChild("Settings") as Button;
        btnCredits = MainMenu.FindChild("Credits") as Button;
        btnQuit = MainMenu.FindChild("Quit") as Button;

        btnNewGame.Pressed += StartGame;
        btnSettings.Pressed += () => { GD.Print("Open Settings"); };
        btnCredits.Pressed += () => { GD.Print("Open Credits"); };
        btnQuit.Pressed += () => { GetTree().Quit(); };
    }

    private void StartGame()
    {
        MainMenu.Visible = false;
        IngameCamera.MoveToFocusHexRow(5, true);
        TransitionCamera.Transition(MainMenuCamera, IngameCamera, 1);
        RoundManager.StartGame();
        TransitionCamera.TransitionFinished += () =>
        {
            IngameUI.Visible = true;
            RoundManager.IsRunning = true;
        };
    }
}

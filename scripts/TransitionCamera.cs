using System;
using Godot;

public partial class TransitionCamera : Node
{
    [Export] public Camera3D Camera { get; set; }

    private bool inTransit = false;

    public event Action TransitionFinished;

    public void Transition(Camera3D from, Camera3D to, float duration)
    {
        if (inTransit)
            return;

        Camera.Fov = from.Fov;
        Camera.GlobalTransform = from.GlobalTransform;
        Camera.Current = true;
        inTransit = true;

        var tween = GetTree().CreateTween()
                             .SetTrans(Tween.TransitionType.Cubic)
                             .SetEase(Tween.EaseType.Out);
        tween.TweenProperty(Camera, "global_position", to.GlobalPosition, duration);
        tween.Parallel().TweenProperty(Camera, "rotation", to.Rotation, duration);
        
        tween.Finished += () =>
        {
            Camera.Current = false;
            to.Current = true;
            inTransit = false;
            OnTransitionFinished();
            tween.Dispose();
        };
    }

    private void OnTransitionFinished()
    {
        TransitionFinished?.Invoke();
        TransitionFinished = null;
    }
}

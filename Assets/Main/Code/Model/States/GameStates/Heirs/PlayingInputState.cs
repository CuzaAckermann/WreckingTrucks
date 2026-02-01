using System;

public class PlayingInputState : InputState
{
    private readonly PlayingInput _playingInput;

    public PlayingInputState(PlayingInput playingInput)
    {
        _playingInput = playingInput ?? throw new ArgumentNullException(nameof(playingInput));
    }

    public override void Update()
    {
        _playingInput.Update();
    }
}
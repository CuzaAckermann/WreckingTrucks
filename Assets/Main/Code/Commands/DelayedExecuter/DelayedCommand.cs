using System;

public class DelayedCommand
{
    private readonly Stopwatch _stopwatch;
    private readonly Command _command;

    public DelayedCommand(Stopwatch stopwatch, Command command)
    {
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        _command = command ?? throw new ArgumentNullException(nameof(command));

        _command.Canceled += FinishWork;
    }

    public void Start()
    {
        _stopwatch.SetNotificationInterval(_command.Delay);
        _stopwatch.IntervalPassed += Execute;

        _stopwatch.Start();
    }

    private void Execute()
    {
        _command.Canceled -= FinishWork;

        _stopwatch.IntervalPassed -= Execute;
        _stopwatch.Stop();

        _command.Execute();

        _stopwatch.Destroy();
    }

    private void FinishWork(Command _)
    {
        _command.Canceled -= FinishWork;

        _stopwatch.IntervalPassed -= Execute;
        _stopwatch.Stop();
        _stopwatch.Destroy();
    }
}
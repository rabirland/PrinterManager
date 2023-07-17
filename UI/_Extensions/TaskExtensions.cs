namespace UI;

public static class TaskExtensions
{
    /// <summary>
    /// Monitors a background task and if finishes with an error, closes the application.
    /// </summary>
    /// <param name="task">The task to monitor.</param>
    public static void Monitor(this Task task)
    {
        task.ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Application.Exit();
            }
        });
    }
}

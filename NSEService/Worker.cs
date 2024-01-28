using System.Diagnostics;

namespace NSEService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Check if the current time is within the desired time window and it's a weekday
                if (IsWithinTimeWindow() && IsWeekday())
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    }
                    // Execute the Python script
                    ExecutePythonScript();
                }

                // Wait for the next iteration
                await Task.Delay(5 * 60 * 1000, stoppingToken);
            }
        }
        private bool IsWithinTimeWindow()
        {
            // Define the time window (9 AM to 3:40 PM)
            TimeSpan startTime = new TimeSpan(9, 0, 0);
            TimeSpan endTime = new TimeSpan(15, 40, 0);

            // Get the current time
            TimeSpan currentTime = DateTimeOffset.Now.TimeOfDay;

            // Check if the current time is within the time window
            return currentTime >= startTime && currentTime <= endTime;
        }

        private bool IsWeekday()
        {
            // Check if the current day is a weekday (Monday to Friday)
            return DateTime.Now.DayOfWeek >= DayOfWeek.Monday && DateTime.Now.DayOfWeek <= DayOfWeek.Friday;
        }
        private void ExecutePythonScript()
        {
            using (Process process = new Process())
            {
                // Set the process start information
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "Services/.venv/scripts/python",
                    Arguments = "Services/main.py", // Replace with the actual path to your Python script
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process.StartInfo = startInfo;

                // Subscribe to the events
                process.OutputDataReceived += (sender, e) => _logger.LogInformation($"Python Output: {e.Data}");
                process.ErrorDataReceived += (sender, e) => _logger.LogError($"Python Error: {e.Data}");

                // Start the process
                process.Start();

                // Begin asynchronous read of the output and error streams
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Wait for the process to exit
                process.WaitForExit();

                // Log the exit code
                _logger.LogInformation($"Python script exited with code {process.ExitCode}");
            }
        }
    }
}

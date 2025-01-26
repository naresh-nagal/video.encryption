using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HLS.MediaService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly ILogger<MediaController> _logger;
        private readonly IConfiguration _configuration;

        public MediaController(IConfiguration configuration, ILogger<MediaController> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertToM3U8([FromForm] IFormFile videoFile)
        {
            if (videoFile == null || videoFile.Length == 0)
                return BadRequest("No video file uploaded.");

            try
            {
                // Temp file path to store the uploaded MP4 file
                string tempFilePath = Path.Combine(Path.GetTempPath(), videoFile.FileName);
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(stream);
                }

                // Set paths for the output and key information file
                string outputDirectory = System.Environment.CurrentDirectory + (_configuration["OutputDirectory"]);
                string outputM3U8 = Path.Combine(outputDirectory, "output.m3u8");
                string keyInfoFile = System.Environment.CurrentDirectory + _configuration["KeyInfoFile"];

                // Run FFmpeg to convert the video to encrypted HLS format
                string ffmpegPath = System.Environment.CurrentDirectory + _configuration["FFmpegPath"];
                string ffmpegArguments = $"-i \"{tempFilePath}\" -hls_key_info_file \"{keyInfoFile}\" -hls_playlist_type vod -hls_time 2 -hls_list_size 0 -f hls \"{outputM3U8}\"";

                bool result = await RunFFmpegAsync(ffmpegPath, ffmpegArguments);

                // Clean up the temporary file
                System.IO.File.Delete(tempFilePath);

                if (result)
                {
                    return Ok(new { Message = "Video converted successfully", M3U8FilePath = outputM3U8 });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while processing the video.");
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private async Task<bool> RunFFmpegAsync(string ffmpegPath, string arguments)
        {
            // Set up the FFmpeg process
            var processStartInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = processStartInfo };

            process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                await process.WaitForExitAsync(cts.Token);
                Console.WriteLine($"Process completed with exit code {process.ExitCode}");
                return process.ExitCode == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}

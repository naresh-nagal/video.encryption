using Microsoft.AspNetCore.Mvc;

namespace HLS.MediaService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HLSController : ControllerBase
    {
        private readonly string _segmentsFolderPath = "m3u8"; // Path for m3u8 and ts files

        public HLSController() { }

        // Endpoint to serve the m3u8 playlist
        [HttpGet("playlist.m3u8")]
        public IActionResult GetPlaylist()
        {
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), _segmentsFolderPath)))
            {
                return NotFound("Segments folder does not exist.");
            }

            // Get all .m3u8 files from the folder
            var m3u8File = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), _segmentsFolderPath), "*.m3u8")
                .OrderBy(file => file)
                .ToList();

            // Get all .ts files from the folder
            var segmentFiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), _segmentsFolderPath), "*.ts")
                .OrderBy(file => file)
                .ToList();

            if (m3u8File.Count == 0)
            {
                return NotFound("No .m3u8 files found.");
            }

            if (segmentFiles.Count == 0)
            {
                return NotFound("No .ts files found.");
            }

            //read m3u8 file and replace segment urls
            var m3u8Content = System.IO.File.ReadAllText(m3u8File[0]);

            //replace segments url to serve the segments using api
            m3u8Content = m3u8Content.Replace("output", "segments/output");
            
            return Content(m3u8Content, "application/vnd.apple.mpegurl");
        }

        // Endpoint to serve .ts files
        [HttpGet("segments/{fileName}")]
        public IActionResult GetSegment(string fileName)
        {
            var filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), _segmentsFolderPath), fileName);

            if (System.IO.File.Exists(filePath))
            {
                return PhysicalFile(filePath, "video/MP2T"); // MIME type for .ts files
            }
            else
            {
                return NotFound();
            }
        }
    }
}

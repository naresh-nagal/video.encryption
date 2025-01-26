using Microsoft.AspNetCore.Mvc;

namespace HLS.MediaService.Controllers
{
    [Route("api/encryption")]
    [ApiController]
    public class EncryptionController : ControllerBase
    {
        private readonly KeyGenerationService _keyGenerationService;

        public EncryptionController()
        {
            _keyGenerationService = new KeyGenerationService(); // Simple instance, in production use DI
        }

        // Endpoint to generate AES-128 encryption key and IV
        // Generate the Kye and IV. Store the key in encryption.key file, and set IV in key_uri file
        [HttpGet("generate-key")]
        public IActionResult GenerateKey()
        {
            var (encryptionKey, iv) = _keyGenerationService.GenerateEncryptionKey();

            // Returning as JSON 
            return Ok(new
            {
                EncryptionKey = encryptionKey,
                IV = iv
            });
        }

        [HttpGet("get-key")]
        public IActionResult GetKey()
        {
            //retrive key from key file
            var keyFile = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "key"), "encryption.key")
                            .OrderBy(file => file)
                            .FirstOrDefault();

            //read key file 
            var key = System.IO.File.ReadAllText(keyFile);

            //return key
            return Ok(key);
        }
    }
}

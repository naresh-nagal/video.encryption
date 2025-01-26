**Video Encryption and Streaming Supported by M3U8 Format Using FFmpeg**

**How it works**

**Install FFmpeg on Your Local Computer:**
1. URL: https://www.gyan.dev/ffmpeg/builds/
2. File: ffmpeg-git-full.7z

**Installation Process:**
1. Download the latest version of ffmpeg-git-full.7z from the URL above and unzip it. Copy the unzipped folder to the C:\ drive (or your preferred location).
2. Navigate to the bin folder inside the unzipped directory and verify that ffmpeg.exe is present.
3. Copy the path containing ffmpeg.exe and add it to your **PATH environment variable**. Save the changes to the environment variable.
4. Additionally, **copy** the ffmpeg.exe file to the HLS.MediaService\m3u8 folder in your project directory.

**How to Convert MP4 to M3U8 Format:**
1. Open the HLS.MediaService API project in Visual Studio (e.g., VS 2022) and run it. The application should be accessible at http://localhost:17663/.
2. Use the API endpoint /media/convert for MP4-to-M3U8 conversion.
3. Use **Postman** for MP4 conversion:
  a) Copy the API URL and paste it into Postman. Request type is **Post**.
  b) Upload the MP4 file using **videoFile** request parameter. Navigate to the **Body** tab in Postman, select **form-data**, and upload the file with the variable name videoFile (Type: File).
4. Send the request. The MP4 file will be converted into .ts segments and saved in the HLS.MediaService/m3u8 folder.

**How to Play the Encrypted Video:**
1. Open the React app in **VS Code**.
2. Run the "npm install" command to install dependencies.
3. Install the required packages:
  a) npm install react-router-dom
  b) npm install hls.js
4. Start the app using the "npm start", the app will run on http://localhost:3000.
5. Click on the **Play Video** link to start playing the video.

**Optional: Generate Your Own Encryption Key**
If you want to generate your own encryption key, follow these steps:
1. Use the API endpoint "/api/encryption/generate-key" to generate a new encryption key and IV (Initialization Vector).
2. Save the generated encryption key in the "key/encryption.key" file and the IV in the "key/key_url.txt" file.
3. The "key_url.txt" file is used by FFmpeg during the encryption process and should contain the following content:
    http://localhost:17663/api/encryption/get-key  # API URL used by the HLS Player (in the React app) to retrieve the encryption key.
    key/encryption.key                            # File path required by FFmpeg to fetch the encryption key during encryption.
    578c0e1faf0f0270720ff748acf608c7              # Replace this with your generated IV key.

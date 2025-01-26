import React from "react";
import HlsPlayer from "./HlsPlayer"; // Assuming you have this component already

const VideoPlayerPage = () => {
  const videoUrl = "http://localhost:17663/api/hls/playlist.m3u8"; // Replace with your HLS stream URL

  return (
    <div>
      <h1>HLS Video Player</h1>
      <HlsPlayer
        videoUrl={videoUrl}
        autoPlay={true}
        controls={true}
        width="800px"
        height="450px"
      />
    </div>
  );
};

export default VideoPlayerPage;

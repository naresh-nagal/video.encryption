import React, { useEffect, useRef } from 'react';
import Hls from 'hls.js';

const HlsPlayer = ({ videoUrl, autoPlay = false, controls = true, width = '100%', height = 'auto' }) => {
  const videoRef = useRef(null);

  useEffect(() => {
    const video = videoRef.current;

    if (Hls.isSupported() && videoUrl) {
      const hls = new Hls({
        xhrSetup: (xhr) => {
          xhr.withCredentials = true; // Send cookies with requests
        },
      });
      hls.loadSource(videoUrl);
      hls.attachMedia(video);

      hls.on(Hls.Events.MANIFEST_PARSED, () => {
        if (autoPlay) {
          video.play();
        }
      });

      return () => {
        hls.destroy();
      };
    } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
      // Native HLS support for Safari
      video.src = videoUrl;
      if (autoPlay) {
        video.play();
      }
    }
  }, [videoUrl, autoPlay]);

  return (
    <div>
      <video
        ref={videoRef}
        controls={controls}
        style={{ width, height }}
      />
    </div>
  );
};

export default HlsPlayer;

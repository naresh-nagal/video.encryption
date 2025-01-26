import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import VideoPlayerPage from "./VideoPlayerPage";

const App = () => {
  return (
    <Router>
      <div>
        <nav>
          <ul>
            <li>
              <Link to="/video-player">Play Video</Link>
            </li>
          </ul>
        </nav>

        <Routes>
          <Route path="/video-player" element={<VideoPlayerPage />} />
        </Routes>
      </div>
    </Router>
  );
};

export default App;
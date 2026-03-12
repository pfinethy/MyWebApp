import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Signup from "./Signup";

function Home() {
  return (
    <div style={{padding:"20px"}}>
      <h1>Welcome to My Web App</h1>

      <ul>
        <li><Link to="/">Home</Link></li>
        <li><Link to="/signup">Sign Up</Link></li>
      </ul>

      <img src="/kitten_on_home.png" width="300" />
    </div>
  );
}

function App() {
  return (
    <Router>
      <Routes>

        <Route path="/" element={<Home />} />

        <Route path="/signup" element={<Signup />} />

      </Routes>
    </Router>
  );
}

export default App;
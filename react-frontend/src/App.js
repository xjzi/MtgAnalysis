import './App.css';

import Links from "./links/Links";
import MainSwitch from './common/MainSwitch';
import { BrowserRouter as Router } from "react-router-dom";

function App() {
  return (
    <div className="app">
      <div className="content">
        <Router>
          <Links/>
          <MainSwitch/>
        </Router>
      </div>
    </div>
  );
}

export default App;

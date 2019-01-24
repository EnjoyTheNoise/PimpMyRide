import React, { Component } from "react";
import "../App.css";

class Home extends Component {
  render() {
    return (
      <div>
        <h2 className="title">Witej w naszym komisie, hej!</h2>
        <h5 className="subtitle">
          Jeśliś niezalogowan, zaloguj, jeśliś niezarejstrowan, zerejstruj, hej!{" "}
          <br />{" "}
        </h5>
        <h5 className="subtitle">
          Po zalogowaniu jeno możesz wechikuły przeglądywać, hej!
        </h5>
      </div>
    );
  }
}

export default Home;

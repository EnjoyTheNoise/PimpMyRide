import React, { Component } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import NavbarContainer from "./containers/NavbarContainer";
import { Switch } from "react-router-dom";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "./App.css";

import LoginFormContainer from "./containers/LoginFormContainer";
import RegisterFormContainer from "./containers/RegisterFormContainer";
import Home from "./components/Home";
import CarsContainer from "./containers/CarsContainer";

class App extends Component {
  componentDidMount = () => {
    if (localStorage.jwtToken) {
      let token = {
        securityToken: localStorage.getItem("jwtToken"),
        refreshToken: localStorage.getItem("refreshToken"),
        expiryDate: localStorage.getItem("Exp")
      };
      this.props.actions.setJwtToken(token);
    } else {
      this.props.actions.setJwtToken(false);
    }
  };
  render() {
    return (
      <Router>
        <div className="app">
          <NavbarContainer />
          <div className="container content">
            <Switch>
              <Route exact path="/" component={Home} />
              <Route path="/login" component={LoginFormContainer} />
              <Route path="/register" component={RegisterFormContainer} />
              <Route path="/cars" component={CarsContainer} />
            </Switch>
          </div>
          <ToastContainer />
        </div>
      </Router>
    );
  }
}

export default App;

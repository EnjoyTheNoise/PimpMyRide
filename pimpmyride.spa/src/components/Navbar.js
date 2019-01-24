import React, { Component } from "react";
import { Link } from "react-router-dom";
import "../App.css";

class Navbar extends Component {
  render() {
    return (
      <nav className="navbar navbar-expand-sm navbar-light bg-light fixed-top">
        <div className="container">
          <Link to="/" className="navbar-brand">
            KomisXD
          </Link>
          <div className="collapse navbar-collapse" id="navbarResponsive">
            <ul className="navbar-nav ml-auto">
              <li className="nav-item">
                <Link className="nav-link" to="/register">
                  Register<span className="sr-only">(current)</span>
                </Link>
              </li>
              {this.props.isLogged ? (
                <React.Fragment>
                  <li className="nav-item">
                    <Link
                      className="nav-link"
                      to="/cars"
                    >
                      Cars<span className="sr-only">(current)</span>
                    </Link>
                  </li>
                  <li className="nav-item">
                    <Link
                      className="nav-link"
                      onClick={this.props.actions.logout}
                      to="/"
                      disabled={this.props.isFetching}
                    >
                      Logout<span className="sr-only">(current)</span>
                    </Link>
                  </li>
                </React.Fragment>
              ) : (
                <li className="nav-item">
                  <Link className="nav-link" to="/login">
                    Login<span className="sr-only">(current)</span>
                  </Link>
                </li>
              )}
            </ul>
          </div>
        </div>
      </nav>
    );
  }
}

export default Navbar;

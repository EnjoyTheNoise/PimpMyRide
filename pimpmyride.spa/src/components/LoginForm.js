import React, { Component } from "react";
import "../styles/Form.css";

class LoginForm extends Component {
  render() {
    return (
      <div className="form-center">
        <div id="Message" className="error error-server text-center">
          {this.props.message === "" ? "" : this.props.message}
        </div>
        <form noValidate>
          <div className="form-group col-md-offset-2">
            <label>Email:</label>
            <input
              className="form-control form-field"
              type="text"
              name="email"
              value={this.props.email}
              onChange={this.props.handleUserInput}
              required
              placeholder="Your email"
            />
          </div>
          <div className="form-group col-md-offset-2">
            <label>Password:</label>
            <input
              className="form-control form-field"
              type="password"
              name="password"
              value={this.props.password}
              onChange={this.props.handleUserInput}
              required
              placeholder="Your password"
            />
          </div>
          <div className="col-md-offset-2">
            <button
              className="btn btn-primary"
              onClick={this.props.onSubmit}
              disabled={this.props.isFetching}
            >
              Login
            </button>
          </div>
        </form>
      </div>
    );
  }
}

export default LoginForm;

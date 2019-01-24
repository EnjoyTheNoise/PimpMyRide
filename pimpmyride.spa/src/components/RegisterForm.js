import React, { Component } from "react";
import "../styles/Form.css";

class RegisterForm extends Component {
  clearErrors = () => {
    try {
      let inputs = document.querySelectorAll("input");
      inputs.forEach(input => {
        input.classList.remove("invalid");
        document.getElementById(input.name).innerHTML = "";
      });
    } catch (error) {
      console.log(
        "Error with clearErrors function in RegisterModal.js: " + error.name
      );
    }
  };

  renderFormErrors = () => {
    try {
      let inputs = document.querySelectorAll("input");
      inputs.forEach(input => {
        if (!this.props[input.name].isValid) {
          input.classList.add("invalid");
        } else {
          input.classList.remove("invalid");
        }
      });
    } catch (error) {
      console.log(
        "Error with renderFromErrors function in RegisterModal.js: " +
          error.name
      );
    }
  };

  render() {
    this.clearErrors();
    this.renderFormErrors();
    return (
      <div className="form-center">
        <form noValidate>
          <div className="form-group col-md-offset-2">
            <label>
              Email:<span style={{ color: "red" }}>*</span>
            </label>
            <input
              className="form-control form-field"
              type="text"
              name="email"
              value={this.props.email.value}
              onChange={this.props.handleUserInput}
              required
              placeholder="Your email"
            />
            <div id="email" className="error" />
            <div className="error">
              {" "}
              {this.props.email.message === ""
                ? null
                : this.props.email.message}
            </div>
          </div>
          <div className="form-group col-md-offset-2">
            <label>
              Password:<span style={{ color: "red" }}>*</span>
            </label>
            <input
              className="form-control form-field"
              type="password"
              name="password"
              value={this.props.password.value}
              onChange={this.props.handleUserInput}
              required
              placeholder="Your password"
            />
            <div id="password" className="error" />
            <div className="error">
              {" "}
              {this.props.password.message === ""
                ? null
                : this.props.password.message}
            </div>
          </div>
          <div className="form-group col-md-offset-2">
            <label>
              Confirm Password:<span style={{ color: "red" }}>*</span>
            </label>
            <input
              className="form-control form-field"
              type="password"
              name="confirmPassword"
              value={this.props.confirmPassword.value}
              onChange={this.props.handleUserInput}
              required
              placeholder="Your email"
            />
            <div id="confirmPassword" className="error" />
            <div className="error">
              {" "}
              {this.props.confirmPassword.message === ""
                ? null
                : this.props.confirmPassword.message}
            </div>
          </div>
          <div className="col info">
            <span style={{ color: "red" }}>*</span> - field required
          </div>
          <div className="col-md-offset-2">
            <button
              className="btn btn-primary"
              onClick={this.props.onSubmit}
              disabled={this.props.isFetching}
            >
              Register
            </button>
          </div>
        </form>
      </div>
    );
  }
}

export default RegisterForm;

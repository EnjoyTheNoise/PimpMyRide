import React, { Component } from "react";
import validator from "validator";
import RegisterForm from "../components/RegisterForm";
import { register, resetErrors } from "../actions/registerAction";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import PropTypes from "prop-types";

const initialState = {
  email: { value: "", isValid: true, message: "" },
  password: { value: "", isValid: true, message: "" },
  confirmPassword: { value: "", isValid: true, message: "" }
};

class RegisterModalContainer extends Component {
  constructor(props) {
    super(props);
    this.state = initialState;
  }

  resetState = () => {
    this.setState({
      email: { value: "", isValid: true, message: "" },
      password: { value: "", isValid: true, message: "" },
      confirmPassword: { value: "", isValid: true, message: "" }
    });
  };

  handleUserInput = e => {
    let state = this.state;
    state[e.target.name].value = e.target.value;
    this.setState(state);
  };

  resetValidation = () => {
    let state = this.state;

    Object.keys(state).map(key => {
      if (state[key].hasOwnProperty("isValid")) {
        state[key].isValid = true;
        state[key].message = "";
      }
      return true;
    });

    this.setState(state);
  };

  validateForm = () => {
    let state = this.state;
    let email = state.email;
    let password = state.password;
    let confirmPassword = state.confirmPassword;

    if (
      validator.isEmpty(email.value) ||
      validator.contains(email.value, " ") ||
      !validator.isEmail(email.value) ||
      !validator.isLength(email.value, { min: 5, max: 254 })
    ) {
      email.isValid = false;
      email.message = "Email format is incorrect";
    }

    if (
      validator.isEmpty(password.value) ||
      validator.contains(password.value, " ") ||
      !validator.isLength(password.value, { max: 40 }) ||
      !password.value.match(
        /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*\\])(?=.{8,})/
      )
    ) {
      password.isValid = false;
      password.message =
        "Password must contain 8-40 characters, including uppercase, lowercase, special character and number";
    }

    if (
      !validator.equals(confirmPassword.value, password.value) ||
      validator.isEmpty(confirmPassword.value)
    ) {
      confirmPassword.isValid = false;
      confirmPassword.message = "Passwords are not the same";
    }

    this.setState(state);

    return email.isValid && password.isValid && confirmPassword.isValid;
  };

  onSubmit = e => {
    e.preventDefault();
    this.resetValidation();
    this.props.actions.resetErrors();

    if (this.validateForm()) {
      this.props.actions.register(
        this.state.email.value,
        this.state.password.value,
        this.state.confirmPassword.value,
        this.context.router
      ).then(() => {
        this.context.router.history.push("/login");
      });
    }
  };

  render() {
    let { email, password, confirmPassword, username } = this.state;
    return (
      <RegisterForm
        email={email}
        password={password}
        confirmPassword={confirmPassword}
        username={username}
        onSubmit={this.onSubmit}
        handleUserInput={this.handleUserInput}
      />
    );
  }
}

const mapStateToProps = state => ({
  isFetching: state.register.isFetching,
  errors: state.register.errors,
  id: state.register.id
});

const mapDispatchToProps = dispatch => ({
  actions: bindActionCreators({ register, resetErrors }, dispatch)
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(RegisterModalContainer);

RegisterModalContainer.contextTypes = {
  router: PropTypes.object.isRequired
};

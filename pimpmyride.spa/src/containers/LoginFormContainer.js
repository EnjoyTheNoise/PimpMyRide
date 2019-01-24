import React, { Component } from "react";
import validator from "validator";
import LoginForm from "../components/LoginForm";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { login } from "../actions/loginAction";
import PropTypes from "prop-types";

const initialState = {
  email: { value: "", isValid: false },
  password: { value: "", isValid: false },
  message: ""
};

class LoginFormContainer extends Component {
  constructor(props) {
    super(props);
    this.state = initialState;
  }

  componentWillUnmount = () => {
    this.resetState();
  };

  resetState = () => {
    let state = this.state;
    state.email.value = "";
    state.email.isValid = false;
    state.password.value = "";
    state.password.isValid = false;
    state.message = "";
    this.setState(state);
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
      }
      return true;
    });
    state.message = "";

    this.setState(state);
  };

  validateForm = () => {
    let state = this.state;
    let password = state.password;
    let email = state.email;

    if (
      validator.isEmpty(email.value) ||
      validator.contains(email.value, " ") ||
      !validator.isEmail(email.value) ||
      !validator.isLength(email.value, { min: 5, max: 254 })
    ) {
      state.email.isValid = false;
      state.message = "Incorrect email or password";
      this.setState(state);
      return false;
    }

    if (
      validator.isEmpty(password.value) ||
      validator.contains(email.value, " ")
    ) {
      state.password.isValid = false;
      state.message = "Incorrect email or password";
      this.setState(state);
      return false;
    }

    return true;
  };

  onSubmit = e => {
    e.preventDefault();
    this.resetValidation();

    if (this.validateForm()) {
      this.props.actions
        .login(
          this.state.email.value,
          this.state.password.value,
          this.context.router
        )
        .then(() => {
          this.context.router.history.push("/");
        });
    }
  };

  render() {
    return (
      <LoginForm
        email={this.state.email.value}
        password={this.state.password.value}
        message={this.state.message}
        onSubmit={this.onSubmit}
        handleUserInput={this.handleUserInput}
        isFetching={this.props.isFetching}
      />
    );
  }
}

const mapStateToProps = state => ({
  isFetching: state.login.isFetching,
  isLogged: state.login.isLogged,
  error: state.login.error
});

const mapDispatchToProps = dispatch => ({
  actions: bindActionCreators({ login }, dispatch)
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(LoginFormContainer);

LoginFormContainer.contextTypes = {
  router: PropTypes.object.isRequired
};

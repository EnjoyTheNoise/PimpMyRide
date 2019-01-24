import React, { Component } from "react";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import App from "./App";
import { setJwtToken } from "./actions/loginAction";

class AppContainer extends Component {
  render() {
    return (
      <App
        isLogged={this.props.isLogged}
        token={this.props.token}
        actions={this.props.actions}
      />
    );
  }
}

const mapStateToProps = state => ({
  isLogged: state.login.isLogged,
  token: state.login.securityToken
});

const mapDispatchToProps = dispatch => ({
  actions: bindActionCreators({ setJwtToken }, dispatch)
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(AppContainer);
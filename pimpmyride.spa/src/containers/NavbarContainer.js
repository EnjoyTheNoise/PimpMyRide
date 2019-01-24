import React, { Component } from "react";
import Navbar from "../components/Navbar";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { logout } from "../actions/loginAction";
import PropTypes from "prop-types";

class NavbarContainer extends Component {
  onLogout = () => {};
  render() {
    return (
      <Navbar
        onLogout={this.onLogout}
        isLogged={this.props.isLogged}
        isFetching={this.props.isFetching}
        actions={this.props.actions}
      />
    );
  }
}

const mapStateToProps = state => ({
  isFetching: state.login.isFetching,
  isLogged: state.login.isLogged
});

const mapDispatchToProps = dispatch => ({
  actions: bindActionCreators({ logout }, dispatch)
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(NavbarContainer);

NavbarContainer.contextTypes = {
  router: PropTypes.object.isRequired
};

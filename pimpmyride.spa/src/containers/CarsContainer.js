import React, { Component } from "react";
import Cars from "../components/Cars";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { getAllCars } from "../actions/carsAction";
import PropTypes from "prop-types";

class CarsContainer extends Component {
  componentDidMount = () => {
    this.props.actions.getAllCars(this.context.router);
  };
  render() {
    return <Cars cars={this.props.cars} isFetching={this.props.isFetching} />;
  }
}

const mapStateToProps = state => ({
  isFetching: state.cars.isFetching,
  cars: state.cars.cars
});

const mapDispatchToProps = dispatch => ({
  actions: bindActionCreators({ getAllCars }, dispatch)
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(CarsContainer);

CarsContainer.contextTypes = {
  router: PropTypes.object.isRequired
};

import React, { Component } from "react";
import Car from "../components/Car";

const initialState = {
  isToggled: false
};

class CarContainer extends Component {
  constructor(props) {
    super(props);
    this.state = initialState;
  }

  toggle = () => {
    let state = this.state;
    state.isToggled = !state.isToggled;

    this.setState(state);
  };

  render() {
    return (
      <Car
        modelName={this.props.modelName}
        manufacturer={this.props.manufacturer}
        engineType={this.props.engineType}
        engineCapacity={this.props.engineCapacity}
        priceForDay={this.props.priceForDay}
        priceForHour={this.props.priceForHour}
        mileage={this.props.mileage}
        collateral={this.props.collateral}
        toggle={this.toggle}
        isToggled={this.state.isToggled}
      />
    );
  }
}

export default CarContainer;

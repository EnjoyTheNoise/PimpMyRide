import React, { Component } from "react";
import CarContainer from "../containers/CarContainer";
import "../App.css";

class Cars extends Component {
  mapCarsToPills = () => {
    console.log(this.props);
    return this.props.cars.map(car => (
      <CarContainer
        key={car.modelName}
        modelName={car.modelName}
        manufacturer={car.manufacturer}
        engineType={car.engineType}
        engineCapacity={car.engineCapacity}
        priceForDay={car.priceForDay}
        priceForHour={car.priceForHour}
        mileage={car.mileage}
        collateral={car.collateral}
      />
    ));
  };
  render() {
    return (
      <div>
        <h1 className="title">Samochody nasze:</h1>
        {this.mapCarsToPills()}
      </div>
    );
  }
}

export default Cars;

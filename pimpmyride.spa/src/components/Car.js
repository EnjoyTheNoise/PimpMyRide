import React, { Component } from "react";
import "../App.css";
import "../styles/Car.css";
import car from "../img/car.png";

class Car extends Component {
  render() {
    return (
      <div>
        <div className="container card-car">
          <div className="row bg-faded">
            <div className="col-md-4">
              <div className="card card-block h-100 justify-content-center align-items-center card-transparent">
                <div className="card-img-top img-fluid">
                  <img src={car} alt="Car" />
                </div>
              </div>
            </div>
            <div className="col-md-8" onClick={this.props.toggle}>
              <div className="card card-block h-100 justify-content-center">
                <div className="car-info">
                  <div>Model: {this.props.modelName}</div>
                  <div>Manufacturer: {this.props.manufacturer}</div>
                  <div>Engine type: {this.props.engineType}</div>
                  <div>Capacity: {this.props.engineCapacity}l</div>
                  {this.props.isToggled ? (
                    <div
                      className={
                        this.props.isToggled
                          ? "additional-info visible tracking-in-expand"
                          : "additional-info"
                      }
                    >
                      <div>
                        <b>Additional info:</b>
                      </div>
                      <div>Price For Day: {this.props.priceForDay}$</div>
                      <div>Price For Hour: {this.props.priceForHour}$</div>
                    </div>
                  ) : null}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Car;

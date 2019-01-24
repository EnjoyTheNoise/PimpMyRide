import axios from "axios";
import { BASE_URL } from "../constants";
import { handleHttpError } from "./httpErrorAction";

export const GET_ALL_CARS_START = "GET_ALL_CARS_START";
export const GET_ALL_CARS_SUCCESS = "GET_ALL_CARS_SUCCESS";
export const GET_ALL_CARS_FAILURE = "GET_ALL_CARS_FAILURE";

const getAllCarsSuccess = response => ({
  type: GET_ALL_CARS_SUCCESS,
  payload: response.data.result
});

const getAllCarsFailure = error => ({
  type: GET_ALL_CARS_FAILURE,
  error
});

export const getAllCars = props => dispatch => {
  dispatch({ type: GET_ALL_CARS_START });

  return axios.get(BASE_URL + "car/").then(
    response => dispatch(getAllCarsSuccess(response)),
    error => {
      if (error.response.status === 400) {
        dispatch(getAllCarsFailure(error));
      } else {
        dispatch(handleHttpError(error, props));
      }
    }
  );
};

import {
  GET_ALL_CARS_START,
  GET_ALL_CARS_SUCCESS,
  GET_ALL_CARS_FAILURE
} from "../actions/carsAction";

const initialState = {
  isFetching: false,
  cars: [],
  error: null
};

export default function cars(state = initialState, action) {
  switch (action.type) {
    case GET_ALL_CARS_START:
      return {
        ...state,
        isFetching: true,
        cars: []
      };
    case GET_ALL_CARS_SUCCESS:
      return {
        ...state,
        isFetching: false,
        cars: action.payload
      };
    case GET_ALL_CARS_FAILURE:
      return {
        ...state,
        isFetching: false,
        error: action.error.response.data
      };
    default:
      return state;
  }
}

import {
    HTTP_401_ERROR,
    HTTP_404_ERROR,
    HTTP_500_ERROR,
    HTTP_OTHER_ERROR
  } from "../actions/httpErrorAction";
  
  const initialState = {
    errorMessage: ""
  };
  
  export default function httpError(state = initialState, action) {
    switch (action.type) {
      case HTTP_401_ERROR: {
        return { ...state };
      }
      case HTTP_500_ERROR: {
        return { ...state };
      }
      case HTTP_404_ERROR: {
        return { ...state };
      }
      case HTTP_OTHER_ERROR: {
        return { ...state, errorMessage: action.error.response.data };
      }
      default:
        return state;
    }
  }
import {
  REGISTER_START,
  REGISTER_SUCCESS,
  REGISTER_FAILURE,
  RESET_ERRORS
} from "../actions/registerAction";

const initialState = {
  id: null,
  isFetching: false,
  errors: null
};

function registerModal(state = initialState, action) {
  switch (action.type) {
    case REGISTER_START:
      return { ...state, isFetching: true };
    case REGISTER_SUCCESS:
      return {
        ...state,
        id: action.payload.id,
        isFetching: false,
        errors: null
      };
    case REGISTER_FAILURE:
      return {
        ...state,
        isFetching: false,
        errors: action.errors.response.data.errors
      };
    case RESET_ERRORS: {
      return {
        ...state,
        errors: null
      };
    }
    default:
      return state;
  }
}

export default registerModal;

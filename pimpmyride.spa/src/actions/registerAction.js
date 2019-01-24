import axios from "axios";
import { BASE_URL } from "../constants";
import { handleHttpError } from "./httpErrorAction";
import { toast } from "react-toastify";

export const REGISTER_START = "REGISTER_START";
export const REGISTER_SUCCESS = "REGISTER_SUCCESS";
export const REGISTER_FAILURE = "REGISTER_FAILURE";

export const RESET_ERRORS = "RESET_ERRORS";

const registerSucces = response => ({
  type: REGISTER_SUCCESS,
  payload: response.result
});

const registerFailure = errors => ({
  type: REGISTER_FAILURE,
  errors
});

export const register = (
  email,
  password,
  confirmPassword,
  username,
  props
) => dispatch => {
  dispatch({ type: REGISTER_START });
  return axios
    .post(BASE_URL + "user/register", {
      email,
      password,
      confirmPassword,
      username
    })
    .then(
      response => {
        dispatch(registerSucces(response.data));
        toast("You can now login", {
          position: "top-right",
          autoClose: 5000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true
        });
        return Promise.resolve(response);
      },
      errors => {
        if (errors.response.status === 400) {
          dispatch(registerFailure(errors));
        } else {
          dispatch(handleHttpError(errors, props));
        }
        return Promise.reject(errors);
      }
    );
};

export const resetErrors = () => dispatch => {
  dispatch({ type: RESET_ERRORS });
};
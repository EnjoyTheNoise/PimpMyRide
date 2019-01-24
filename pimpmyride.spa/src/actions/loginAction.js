import axios from "axios";
import { BASE_URL } from "../constants";
import { handleHttpError } from "./httpErrorAction";

export const LOGIN_START = "LOGIN_START";
export const LOGIN_SUCCESS = "LOGIN_SUCCESS";
export const LOGIN_FAILURE = "LOGIN_FAILURE";

export const LOGOUT_START = "LOGOUT_START";
export const LOGOUT_SUCCESS = "LOGOUT_SUCCESS";
export const LOGOUT_FAILURE = "LOGOUT_FAILURE";

export const REFRESH_TOKEN_START = "REFRESH_TOKEN_START";
export const REFRESH_TOKEN_SUCCESS = "REFRESH_TOKEN_SUCCESS";
export const REFRESH_TOKEN_FAILURE = "REFRESH_TOKEN_FAILURE";

export const SET_JWT_TOKEN_SUCCESS = "SET_JWT_TOKEN_SUCCESS";
export const SET_JWT_TOKEN_FAILURE = "SET_JWT_TOKEN_FAILURE";

export const LOGIN_MODAL_CLOSE = "LOGIN_MODAL_CLOSE";
export const LOGIN_MODAL_OPEN = "LOGIN_MODAL_OPEN";

const loginSuccess = response => ({
  type: LOGIN_SUCCESS,
  payload: response.data.result
});

const loginFailure = error => ({
  type: LOGIN_FAILURE,
  error
});

export const login = (email, password, props) => dispatch => {
  dispatch({ type: LOGIN_START });
  return axios.post(BASE_URL + "user/login", { email, password }).then(
    response => {
      dispatch(setJwtToken(response.data.result));
      dispatch(loginSuccess(response));
      return Promise.resolve(response);
    },
    error => {
      if (error.response.status === 400) {
        dispatch(loginFailure(error));
      } else {
        dispatch(handleHttpError(error, props));
      }
      return Promise.reject(error);
    }
  );
};

const logoutSuccess = () => ({
  type: LOGOUT_SUCCESS
});

const logoutFailure = error => ({
  type: LOGOUT_FAILURE,
  error
});

export const logout = props => dispatch => {
  dispatch({ type: LOGOUT_START });
  return axios.post(BASE_URL + "user/logout").then(
    () => {
      dispatch(setJwtToken(false));
      dispatch(logoutSuccess());
    },
    error => {
      if (error.response.status === 400) {
        dispatch(logoutFailure(error));
      } else {
        dispatch(handleHttpError(error, props));
      }
    }
  );
};

const tokenRefreshSuccess = response => ({
  type: REFRESH_TOKEN_SUCCESS,
  payload: response.data
});

const tokenRefreshFailure = error => ({
  type: REFRESH_TOKEN_FAILURE,
  error
});

export const refreshToken = () => ({ dispatch, getState }) => {
  dispatch({ type: REFRESH_TOKEN_START });
  return axios
    .post(BASE_URL + "token/refresh", {
      refreshToken: getState().login.refreshToken,
      userId: getState().login.id
    })
    .then(
      response => {
        dispatch(tokenRefreshSuccess(response));
        dispatch(setJwtToken(response.data));
      },
      error => dispatch(tokenRefreshFailure(error))
    );
  // dispatch(setJwtToken(false));
  // dispatch(tokenRefreshFailure("Token expired"));
};

const setJwtSuccess = token => ({
  type: SET_JWT_TOKEN_SUCCESS,
  payload: token
});

const setJwtFailure = () => ({
  type: SET_JWT_TOKEN_FAILURE
});

export const setJwtToken = token => dispatch => {
  if (token) {
    localStorage.setItem("jwtToken", token.securityToken);
    localStorage.setItem("refreshToken", token.refreshToken);
    axios.defaults.headers.common["Authorization"] =
      "Bearer " + token.securityToken;
    dispatch(setJwtSuccess(token));
  } else {
    localStorage.removeItem("jwtToken");
    localStorage.removeItem("refreshToken");
    delete axios.defaults.headers.common["Authorization"];
    dispatch(setJwtFailure());
  }
};

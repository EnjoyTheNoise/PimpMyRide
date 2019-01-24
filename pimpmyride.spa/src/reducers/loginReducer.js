import {
    LOGIN_START,
    LOGIN_SUCCESS,
    LOGIN_FAILURE,
    LOGOUT_START,
    LOGOUT_SUCCESS,
    LOGOUT_FAILURE,
    REFRESH_TOKEN_START,
    REFRESH_TOKEN_SUCCESS,
    REFRESH_TOKEN_FAILURE,
    SET_JWT_TOKEN_SUCCESS,
    SET_JWT_TOKEN_FAILURE,
  } from "../actions/loginAction";
  import jwt from "jsonwebtoken";
  
  const initialState = {
    id: null,
    token: null,
    refreshToken: null,
    exp: null,
    isFetching: false,
    isLogged: false,
    error: null,
    modal: false
  };
  
  function login(state = initialState, action) {
    switch (action.type) {
      case LOGIN_START:
        return { ...state, isFetching: true, error: null };
      case LOGIN_SUCCESS:
        return {
          ...state,
          id: jwt.decode(action.payload.securityToken)[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
          ],
          token: action.payload.securityToken,
          refreshToken: action.payload.refreshToken,
          exp: action.payload.expiryDate,
          isLogged: true,
          isFetching: false,
          error: null
        };
      case LOGIN_FAILURE:
        return {
          ...state,
          isFetching: false,
          isLogged: false,
          error: action.error.response.data.errors
        };
      case LOGOUT_START:
        return { ...state, isFetching: true };
      case LOGOUT_SUCCESS:
        return {
          ...state,
          token: null,
          refreshToken: null,
          exp: null,
          isLogged: false,
          isFetching: false
        };
      case LOGOUT_FAILURE:
        return {
          ...state,
          isFetching: false,
          isLogged: true,
          error: action.error.response.data.errors
        };
      case REFRESH_TOKEN_START:
        return {
          ...state,
          isFetching: true
        };
      case REFRESH_TOKEN_SUCCESS:
        return {
          ...state,
          token: action.payload.securityToken,
          refreshToken: action.payload.refreshToken,
          exp: action.payload.expiryDate,
          isFetching: false
        };
      case REFRESH_TOKEN_FAILURE:
        return {
          ...state,
          id: null,
          token: null,
          refreshToken: null,
          exp: null,
          isLogged: false,
          isFetching: false,
          error: action.error
        };
      case SET_JWT_TOKEN_SUCCESS:
        return {
          ...state,
          id: jwt.decode(action.payload.securityToken)[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
          ],
          token: action.payload.securityToken,
          refreshToken: action.payload.refreshToken,
          exp: jwt.decode(action.payload.securityToken)["exp"],
          isLogged: true
        };
      case SET_JWT_TOKEN_FAILURE:
        return {
          ...state,
          id: null,
          token: null,
          refreshToken: null,
          exp: null,
          isLogged: false
        };
      default:
        return state;
    }
  }
  
  export default login;
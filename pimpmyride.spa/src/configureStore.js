import { createStore, compose, applyMiddleware, combineReducers } from "redux";

import thunkMiddleware from "redux-thunk";
import rootReducer from "./rootReducer";

export default function configureStore(initialState) {
  const composeEnhancers =
    (typeof window === "object" &&
      window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__) ||
    compose;
  return createStore(
    combineReducers({
      ...rootReducer
    }),
    initialState,
    composeEnhancers(applyMiddleware(thunkMiddleware))
  );
}
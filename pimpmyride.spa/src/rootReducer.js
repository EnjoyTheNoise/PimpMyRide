import login from "./reducers/loginReducer";
import httpError from "./reducers/httpErrorReducer";
import register from "./reducers/registerReducer";
import cars from "./reducers/carsReducer";

const rootReducer = {
  login,
  httpError,
  register,
  cars,
};

export default rootReducer;

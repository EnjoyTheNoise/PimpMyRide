export const HTTP_401_ERROR = "HTTP_401_ERROR";
export const HTTP_404_ERROR = "HTTP_404_ERROR";
export const HTTP_500_ERROR = "HTTP_500_ERROR";
export const HTTP_OTHER_ERROR = "HTTP_OTHER_ERROR";

const execute401 = () => ({
  type: HTTP_401_ERROR
});

const execute404 = () => ({
  type: HTTP_404_ERROR
});

const execute500 = () => ({
  type: HTTP_500_ERROR
});

const executeOtherError = error => ({
  type: HTTP_OTHER_ERROR,
  error: error
});

export const handleHttpError = (error, props) => {
  switch (error.response.status) {
    case 401: {
      props.history.push("/401");
      return execute401();
    }
    case 404: {
      props.history.push("/404");
      return execute404();
    }
    case 500: {
      props.history.push("/500");
      return execute500();
    }
    default:
      props.history.push("/error");
      return executeOtherError(error);
  }
};
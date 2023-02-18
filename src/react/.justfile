environment := "development"

react-local-up:
  REACT_APP_ENVIRONMENT_NAME={{environment}} \
  NODE_ENV={{environment}} \
  PORT=3000 \
  REACT_APP_KEYCLOAK_CLIENT_ID=react \
  REACT_APP_KEYCLOAK_URL=http://localhost:52433/realms/react-example \
  REACT_APP_BACKEND_URL=http://localhost:52434 \
  yarn start

react-kube-up:
  REACT_APP_ENVIRONMENT_NAME={{environment}} \
  NODE_ENV={{environment}} \
  PORT=3000 \
  REACT_APP_KEYCLOAK_CLIENT_ID=react \
  REACT_APP_KEYCLOAK_URL=http://keycloak.asp-react-oidc.test/realms/react-example \
  REACT_APP_BACKEND_URL=http://backend.asp-react-oidc.test \
  yarn start
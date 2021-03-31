import {KeycloakConfig} from 'keycloak-angular';

// Add here your keycloak setup infos
const keycloakConfig: KeycloakConfig = {
  url: 'http://10.224.22.28:8080/auth',
  realm: 'OER',
  clientId: 'OER-Client',
  credentials: {
    secret: '68ab147e-4847-48e6-b4e3-70fa8ad6bef4'
  }
};

export const environment = {
  production: true,
  keycloak: keycloakConfig,
  clientUrl: 'http://10.224.4.12',
  adminUrl: 'http://10.224.4.12:8080/',
  apiUrl: 'http://10.224.22.28/api/',
  serverUrl: 'http://10.224.22.28',
  encKey: 'rNyuFzkutcZL6kmFqWBEtGzJksun1ijU'
};

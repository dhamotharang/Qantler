import {KeycloakConfig} from 'keycloak-angular';

// Add here your keycloak setup infos
const keycloakConfig: KeycloakConfig = {
  url: 'http://182.72.164.238:8080/auth',
  realm: 'OER',
  clientId: 'OER-SIT-Client',
  credentials: {
    secret: '7e19fa2d-bc21-4c87-b7c6-6dde4472ca95'
  }
};

export const environment = {
  production: true,
  keycloak: keycloakConfig,
  clientUrl: 'http://182.72.164.238:7000',
  adminUrl: 'http://182.72.164.238:7001/',
  apiUrl: 'http://182.72.164.238:7040/api/',
  serverUrl: 'http://182.72.164.238:7040',
  encKey: 'rNyuFzkutcZL6kmFqWBEtGzJksun1ijU'
};
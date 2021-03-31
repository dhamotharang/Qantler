import {KeycloakConfig} from 'keycloak-angular';

// Add here your keycloak setup infos
const keycloakConfig: KeycloakConfig = {
  url: 'http://10.224.22.28:8080/auth',
  realm: 'OER',
  clientId: 'OER-Admin',
  credentials: {
    secret: 'c1c57e85-df13-4ac3-8db1-4ca732a7955d'
  }
};

export const environment = {
  production: true,
  keycloak: keycloakConfig,
  clientUrl: 'http://10.224.4.12:8080',
  apiUrl: 'http://10.224.22.28/api/',
  encKey: 'rNyuFzkutcZL6kmFqWBEtGzJksun1ijU',
  userClientUrl: 'http://10.224.4.12/'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.

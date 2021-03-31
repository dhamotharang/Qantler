// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import {KeycloakConfig} from 'keycloak-angular';

// Add here your keycloak setup infos
const keycloakConfig: KeycloakConfig = {
  url: 'http://182.72.164.238:8080/auth',
  realm: 'OER',
  clientId: 'OER-SIT-Admin',
  credentials: {
    secret: 'adf1fd36-fa68-4ae5-b884-b1c379bc75dd'
  }
};

export const environment = {
  production: false,
  keycloak: keycloakConfig,
  clientUrl: 'http://localhost:4200',
  apiUrl: 'https://localhost:44371/api/',
  encKey: 'rNyuFzkutcZL6kmFqWBEtGzJksun1ijU',
  userClientUrl: 'http://182.72.164.238:7000/'
};
/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.

// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import {KeycloakConfig} from 'keycloak-angular';

// Add here your keycloak setup infos
const keycloakConfig: KeycloakConfig = {
  url: 'http://182.72.164.238:8080/auth',
  realm: 'OER',
  clientId: 'Admin',
  credentials: {
    secret: 'd5e80ee2-18ef-4f1a-a222-50ffedfc25cf'
  }
};

export const environment = {
  production: true,
  keycloak: keycloakConfig,
  clientUrl: 'http://182.72.164.238:8001',
  apiUrl: 'http://182.72.164.238:8040/api/',
  encKey: 'rNyuFzkutcZL6kmFqWBEtGzJksun1ijU',
  userClientUrl: 'http://182.72.164.238:8000/'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.

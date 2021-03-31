import {KeycloakService} from 'keycloak-angular';
import {environment} from '../environments/environment';

/**
 * initialize keycloak initOptions
 *
 * @param keycloak
 */
export function initializer(keycloak: KeycloakService): () => Promise<any> {
  return (): Promise<any> => keycloak.init({
    config: environment.keycloak,
    initOptions: {
      onLoad: 'check-sso',
      checkLoginIframe: true
    },
    enableBearerInterceptor: false,
    bearerExcludedUrls: ['/assets']
  });
}

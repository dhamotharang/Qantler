import { KeycloakService } from 'keycloak-angular';
import { Browser } from 'selenium-webdriver';
import { environment } from '../environments/environment';
/**
 * initialize keycloak initOptions
 * @param keycloak contain keycloak details
 */

class Browsers {
  checkBrowser(): Array<string> {
    const ua = navigator.userAgent;
    let tem,
      M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
    if (/trident/i.test(M[1])) {
      tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
      return ['IE', (tem[1] || '')];
    }
    if (M[1] === 'Chrome') {
      tem = ua.match(/\b(OPR|Edge)\/(\d+)/);
      if (tem != null) {
        return tem.slice(1).join(' ').replace('OPR', 'Opera').split(' ');
      }
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
    if ((tem = ua.match(/version\/(\d+)/i)) != null) { M.splice(1, 1, tem[1]); }
    return [M[0], M[1]];

  }
}

export function initializer(keycloak: KeycloakService): () => Promise<any> {
  const object = (new Browsers()).checkBrowser();
  if ((object[0] === 'Chrome' && Number(object[1]) >= 56) ||
    (object[0] === 'Edge' && Number(object[1]) >= 17) ||
    (object[0] === 'Firefox' && Number(object[1]) >= 51) ||
    (object[0] === 'Safari' && Number(object[1]) >= 10)) {
    return (): Promise<any> => keycloak.init({
      config: environment.keycloak,
      initOptions: {
        onLoad: 'login-required',
        checkLoginIframe: true,
      },
      enableBearerInterceptor: false,
      bearerExcludedUrls: ['/assets', 'assets'],
    });
  } else {
    window.location.href = '/assets/old_browser.html';
    const result = new Promise<any>((resolve) => {
      resolve();
      return true;
     });
  }
}

import {Pipe, PipeTransform} from '@angular/core';
import { DomSanitizer, SafeHtml, SafeStyle, SafeScript, SafeUrl, SafeResourceUrl } from '@angular/platform-browser';

/**
 * Safe URL Pipe
 */
@Pipe({
  name: 'safe'
})
export class SafePipe implements PipeTransform {

  /**
   *
   * @param sanitizer
   */
  constructor(private sanitizer: DomSanitizer) {
  }

  public transform(value: any, type: string): SafeHtml | SafeStyle | SafeScript | SafeUrl | SafeResourceUrl {
    switch (type) {
			case 'html': return this.sanitizer.bypassSecurityTrustHtml(value);
			case 'style': return this.sanitizer.bypassSecurityTrustStyle(value);
			case 'script': return this.sanitizer.bypassSecurityTrustScript(value);
			case 'url': return this.sanitizer.bypassSecurityTrustUrl(value);
			case 'resourceUrl': return this.sanitizer.bypassSecurityTrustResourceUrl(value);
			default: throw new Error(`Invalid safe type specified: ${type}`);
    }
  }

  /**
   * returns snitized URL
   *
   * @param url
   */
}

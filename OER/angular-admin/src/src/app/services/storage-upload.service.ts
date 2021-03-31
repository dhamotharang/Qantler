import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';

/**
 * Storage upload helper service
 */
@Injectable({
  providedIn: 'root'
})

export class StorageUploadService {
  resourceFolder: string;
  profilePicFolder: string;
  thumbnailPicFolder: string;
  courseFolder: string;
  testFolder: string;
  copyrightFolder: any;
  Announcements: string;

  constructor(private http: HttpClient) {
    this.resourceFolder = 'resources/';
    this.courseFolder = 'courses/';
    this.testFolder = 'tests/';
    this.profilePicFolder = 'profile/';
    this.thumbnailPicFolder = 'thumbs/';
    this.copyrightFolder = 'copyright/';
    this.Announcements ='announcements/'
  }

  /**
   * uploads file API
   *
   * @param req contain req details
   */
  upload(req): Observable<any> {
    return this.http.request(req);
  }
}

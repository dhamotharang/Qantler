import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

/**
 * Upload API Service
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
  tempFolder: string;

  /**
   *
   * @param http
   */
  constructor(private http: HttpClient) {
    this.resourceFolder = 'resources/';
    this.courseFolder = 'courses/';
    this.testFolder = 'tests/';
    this.profilePicFolder = 'profile/';
    this.thumbnailPicFolder = 'thumbs/';
    this.tempFolder = 'temp/';
  }

  /**
   * Upload file to dotnet API
   *
   * @param req
   */
  upload(req): Observable<any> {
    return this.http.request(req);
  }
}
